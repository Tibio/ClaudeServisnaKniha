using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.Util.Store;
using ServisnaKniha.Models;
using System.Threading;

namespace ServisnaKniha.Helpers;

public class GoogleDriveHelper
{
    private DriveService? _service;
    private readonly string _tokenPath;
    private string _folderName = "ServisnaKniha_Zalohy";

    public bool JePripojeny => _service != null;

    public GoogleDriveHelper()
    {
        _tokenPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "ServisnaKniha", "google_token");
    }

    public async Task<bool> PripojitAsync(string clientId, string clientSecret, string folderName, CancellationToken ct = default)
    {
        _folderName = folderName;
        var secrets = new ClientSecrets { ClientId = clientId, ClientSecret = clientSecret };

        var credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
            secrets,
            [DriveService.Scope.DriveFile],
            "user",
            ct,
            new FileDataStore(_tokenPath, true));

        _service = new DriveService(new BaseClientService.Initializer
        {
            HttpClientInitializer = credential,
            ApplicationName = "ServisnaKniha"
        });
        return true;
    }

    public void Odpojit()
    {
        _service?.Dispose();
        _service = null;
        // Vymaže uložený token
        if (Directory.Exists(_tokenPath))
            Directory.Delete(_tokenPath, true);
    }

    // ==================== UPLOAD ====================

    public async Task<string> NahratZalohuAsync(string filePath, IProgress<int>? progress = null, CancellationToken ct = default)
    {
        EnsureConnected();
        var folderId = await ZabezpecFolderAsync(ct);
        var fileName = Path.GetFileName(filePath);

        var meta = new Google.Apis.Drive.v3.Data.File
        {
            Name = fileName,
            Parents = [folderId],
            Description = $"Záloha ServisnaKniha — {DateTime.Now:dd.MM.yyyy HH:mm}"
        };

        await using var stream = File.OpenRead(filePath);
        var req = _service!.Files.Create(meta, stream, "application/x-sqlite3");
        req.Fields = "id, name, size, createdTime";

        req.ProgressChanged += p =>
        {
            if (p.Status == UploadStatus.Uploading && stream.Length > 0)
                progress?.Report((int)(p.BytesSent * 100 / stream.Length));
        };

        var result = await req.UploadAsync(ct);
        if (result.Status == UploadStatus.Failed)
            throw new Exception($"Upload zlyhal: {result.Exception?.Message}");

        return req.ResponseBody?.Id ?? "";
    }

    // ==================== ZOZNAM ZALOH ====================

    public async Task<List<DriveZaloha>> GetZoznamZalohAsync(CancellationToken ct = default)
    {
        EnsureConnected();
        var folderId = await NajstFolderIdAsync(ct);
        if (folderId == null) return [];

        var req = _service!.Files.List();
        req.Q = $"'{folderId}' in parents and trashed=false";
        req.Fields = "files(id, name, size, createdTime, description)";
        req.OrderBy = "createdTime desc";
        req.PageSize = 50;

        var result = await req.ExecuteAsync(ct);
        return result.Files.Select(f => new DriveZaloha
        {
            Id = f.Id,
            Nazov = f.Name,
            VelkostBytes = f.SizeAsLong ?? 0,
            DatumVytvorenia = f.CreatedTimeDateTimeOffset?.DateTime ?? DateTime.MinValue,
            Popis = f.Description ?? ""
        }).ToList();
    }

    // ==================== DOWNLOAD ====================

    public async Task StiahnuZalohuAsync(string fileId, string destPath, IProgress<int>? progress = null, CancellationToken ct = default)
    {
        EnsureConnected();
        var req = _service!.Files.Get(fileId);
        req.MediaDownloader.ProgressChanged += p =>
        {
            if (p.Status == Google.Apis.Download.DownloadStatus.Downloading)
                progress?.Report((int)(p.BytesDownloaded * 100 / Math.Max(p.BytesDownloaded, 1)));
        };

        await using var stream = File.Create(destPath);
        await req.DownloadAsync(stream, ct);
    }

    // ==================== VYMAZAT ====================

    public async Task VymazatZalohuAsync(string fileId, CancellationToken ct = default)
    {
        EnsureConnected();
        await _service!.Files.Delete(fileId).ExecuteAsync(ct);
    }

    // ==================== POMOCNÉ ====================

    private async Task<string> ZabezpecFolderAsync(CancellationToken ct)
    {
        var existujuci = await NajstFolderIdAsync(ct);
        if (existujuci != null) return existujuci;

        var folder = new Google.Apis.Drive.v3.Data.File
        {
            Name = _folderName,
            MimeType = "application/vnd.google-apps.folder"
        };
        var req = _service!.Files.Create(folder);
        req.Fields = "id";
        var created = await req.ExecuteAsync(ct);
        return created.Id;
    }

    private async Task<string?> NajstFolderIdAsync(CancellationToken ct)
    {
        var req = _service!.Files.List();
        req.Q = $"name='{_folderName}' and mimeType='application/vnd.google-apps.folder' and trashed=false";
        req.Fields = "files(id)";
        var result = await req.ExecuteAsync(ct);
        return result.Files.FirstOrDefault()?.Id;
    }

    private void EnsureConnected()
    {
        if (_service == null) throw new InvalidOperationException("Google Drive nie je pripojený.");
    }
}

public class DriveZaloha
{
    public string Id { get; set; } = "";
    public string Nazov { get; set; } = "";
    public long VelkostBytes { get; set; }
    public DateTime DatumVytvorenia { get; set; }
    public string Popis { get; set; } = "";
    public string VelkostText => VelkostBytes < 1024 * 1024
        ? $"{VelkostBytes / 1024.0:N1} KB"
        : $"{VelkostBytes / 1024.0 / 1024.0:N2} MB";
}
