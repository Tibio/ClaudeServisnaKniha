using ServisnaKniha.Helpers;
using ServisnaKniha.Models;
using System.Drawing;
using System.Windows.Forms;

namespace ServisnaKniha.Forms;

public partial class ZalohaPanel : UserControl
{
    private readonly string _dbPath;
    private readonly Action _onImport;
    private readonly GoogleDriveHelper _drive = new();
    private AppSettings _settings = AppSettings.Load();

    // Lokálna záloha
    private Label lblPoslednaLok = null!;

    // Google Drive
    private Label lblStavDrive = null!;
    private Button btnPripojit = null!;
    private Button btnNahrat = null!;
    private Button btnStahnut = null!;
    private Button btnVymazatDrive = null!;
    private Button btnObnovitZoznam = null!;
    private ListView lvZalohy = null!;
    private ProgressBar progressBar = null!;
    private Label lblProgress = null!;

    private CancellationTokenSource? _cts;

    public ZalohaPanel(string dbPath, Action onImport)
    {
        _dbPath = dbPath;
        _onImport = onImport;
        InitializeComponent();
        AktualizujStavDrive();
        AktualizujPoslednaLok();
    }

    private void InitializeComponent()
    {
        Dock = DockStyle.Fill;
        Font = new Font("Segoe UI", 9f);
        BackColor = Color.FromArgb(248, 249, 252);

        var main = new TableLayoutPanel
        {
            Dock = DockStyle.Fill, ColumnCount = 2, RowCount = 2,
            Padding = new Padding(16)
        };
        main.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 38));
        main.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 62));
        main.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        main.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));

        // ===== ĽAVÝ PANEL — Lokálna záloha =====
        var grpLok = new GroupBox
        {
            Text = "Lokálna záloha / Obnovenie",
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI", 10f, FontStyle.Bold),
            ForeColor = Color.FromArgb(30, 58, 95),
            Margin = new Padding(0, 0, 10, 0),
            Padding = new Padding(10)
        };

        var pnlLok = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill, FlowDirection = FlowDirection.TopDown,
            WrapContents = false, Padding = new Padding(5, 15, 5, 5), AutoSize = false
        };

        pnlLok.Controls.Add(new Label
        {
            Text = "Záloha uloží aktuálnu databázu\ndo zvoleného priečinka.",
            ForeColor = Color.FromArgb(80, 80, 80),
            AutoSize = true, Margin = new Padding(0, 0, 0, 12)
        });

        var btnExport = VytvorBtn("⬇  Exportovať zálohu...", Color.FromArgb(40, 167, 69), 220);
        btnExport.Click += BtnExport_Click;
        pnlLok.Controls.Add(btnExport);

        pnlLok.Controls.Add(new Label
        {
            Text = "Obnovenie nahradí VŠETKY aktuálne\ndáta zálohou. Nezvratná operácia!",
            ForeColor = Color.FromArgb(160, 50, 0),
            AutoSize = true, Margin = new Padding(0, 18, 0, 8)
        });

        var btnImport = VytvorBtn("⬆  Importovať zálohu...", Color.FromArgb(220, 53, 69), 220);
        btnImport.Click += BtnImport_Click;
        pnlLok.Controls.Add(btnImport);

        pnlLok.Controls.Add(new Label { Height = 16 });

        lblPoslednaLok = new Label
        {
            ForeColor = Color.FromArgb(100, 100, 100),
            AutoSize = true, Font = new Font("Segoe UI", 8.5f)
        };
        pnlLok.Controls.Add(lblPoslednaLok);

        grpLok.Controls.Add(pnlLok);
        main.Controls.Add(grpLok, 0, 0);

        // ===== PRAVÝ PANEL — Google Drive =====
        var grpDrive = new GroupBox
        {
            Text = "Google Drive",
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI", 10f, FontStyle.Bold),
            ForeColor = Color.FromArgb(30, 58, 95),
            Padding = new Padding(10)
        };

        var pnlDrive = new Panel { Dock = DockStyle.Fill };

        // Stavová lišta
        var pnlStatus = new Panel { Dock = DockStyle.Top, Height = 36, BackColor = Color.FromArgb(240, 244, 250) };
        lblStavDrive = new Label
        {
            Dock = DockStyle.Left, Width = 320, TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(8, 0, 0, 0), Font = new Font("Segoe UI", 9f, FontStyle.Bold)
        };
        var pnlBtnsTop = new FlowLayoutPanel
        {
            Dock = DockStyle.Right, Width = 380, FlowDirection = FlowDirection.LeftToRight,
            Padding = new Padding(4, 4, 4, 4)
        };
        btnPripojit = VytvorBtnMaly("Pripojiť", Color.FromArgb(0, 123, 255));
        btnPripojit.Click += BtnPripojit_Click;
        var btnNastavenia = VytvorBtnMaly("⚙ Nastavenia", Color.FromArgb(108, 117, 125));
        btnNastavenia.Click += BtnNastavenia_Click;
        pnlBtnsTop.Controls.AddRange([btnPripojit, btnNastavenia]);
        pnlStatus.Controls.AddRange([lblStavDrive, pnlBtnsTop]);

        // Toolbar zálohy
        var toolbar = new FlowLayoutPanel
        {
            Dock = DockStyle.Top, Height = 42, BackColor = Color.FromArgb(248, 249, 250),
            Padding = new Padding(6, 6, 6, 0), FlowDirection = FlowDirection.LeftToRight
        };
        btnNahrat = VytvorBtnMaly("⬆ Nahrať zálohu", Color.FromArgb(40, 167, 69));
        btnNahrat.Click += BtnNahrat_Click;
        btnObnovitZoznam = VytvorBtnMaly("↺ Obnoviť zoznam", Color.FromArgb(23, 162, 184));
        btnObnovitZoznam.Click += BtnObnovitZoznam_Click;
        btnStahnut = VytvorBtnMaly("⬇ Stiahnuť vybranú", Color.FromArgb(0, 123, 255));
        btnStahnut.Click += BtnStahnut_Click;
        btnVymazatDrive = VytvorBtnMaly("✕ Vymazať", Color.FromArgb(220, 53, 69));
        btnVymazatDrive.Click += BtnVymazat_Click;
        toolbar.Controls.AddRange([btnNahrat, btnObnovitZoznam, btnStahnut, btnVymazatDrive]);

        // Zoznam záloh na Drive
        lvZalohy = new ListView
        {
            Dock = DockStyle.Fill, View = View.Details, FullRowSelect = true,
            GridLines = true, MultiSelect = false,
            Font = new Font("Segoe UI", 9f),
            BackColor = Color.White
        };
        lvZalohy.Columns.Add("Názov súboru", 250);
        lvZalohy.Columns.Add("Dátum nahrania", 135);
        lvZalohy.Columns.Add("Veľkosť", 80);
        lvZalohy.Columns.Add("Popis", 200);

        // Progress bar
        var pnlProgress = new Panel { Dock = DockStyle.Bottom, Height = 28, BackColor = Color.White };
        progressBar = new ProgressBar { Dock = DockStyle.Left, Width = 300, Height = 18, Margin = new Padding(5), Visible = false };
        lblProgress = new Label { Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft, Padding = new Padding(310, 0, 0, 0) };
        pnlProgress.Controls.AddRange([progressBar, lblProgress]);

        pnlDrive.Controls.Add(lvZalohy);
        pnlDrive.Controls.Add(toolbar);
        pnlDrive.Controls.Add(pnlStatus);
        pnlDrive.Controls.Add(pnlProgress);
        grpDrive.Controls.Add(pnlDrive);
        main.Controls.Add(grpDrive, 1, 0);

        // Spodný informačný panel
        var pnlInfo = new Panel
        {
            Dock = DockStyle.Fill, BackColor = Color.FromArgb(232, 245, 232),
            Padding = new Padding(10, 8, 10, 8)
        };
        pnlInfo.Controls.Add(new Label
        {
            Text = "ℹ  Export zálohy = kópia databázového súboru (.db). Pre Google Drive je potrebné zadať vlastný Client ID a Client Secret (bezplatne cez Google Cloud Console).",
            Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft,
            ForeColor = Color.FromArgb(20, 80, 20)
        });
        main.Controls.Add(pnlInfo, 0, 1);
        main.SetColumnSpan(pnlInfo, 2);

        Controls.Add(main);
    }

    // ==================== LOKÁLNA ZÁLOHA ====================

    private void BtnExport_Click(object? sender, EventArgs e)
    {
        using var dlg = new SaveFileDialog
        {
            Title = "Uložiť zálohu databázy",
            Filter = "Záloha databázy (*.db)|*.db|Všetky súbory (*.*)|*.*",
            FileName = $"ServisnaKniha_zaloha_{DateTime.Now:yyyy-MM-dd_HHmm}.db",
            DefaultExt = "db"
        };
        if (dlg.ShowDialog() != DialogResult.OK) return;
        try
        {
            File.Copy(_dbPath, dlg.FileName, overwrite: true);
            _settings.LastLocalBackup = DateTime.Now;
            _settings.Save();
            AktualizujPoslednaLok();
            MessageBox.Show($"Záloha bola uložená:\n{dlg.FileName}", "Záloha úspešná",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Chyba pri zálohe:\n{ex.Message}", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnImport_Click(object? sender, EventArgs e)
    {
        using var dlg = new OpenFileDialog
        {
            Title = "Vybrať zálohu na obnovenie",
            Filter = "Databázový súbor (*.db)|*.db|Všetky súbory (*.*)|*.*"
        };
        if (dlg.ShowDialog() != DialogResult.OK) return;

        if (MessageBox.Show(
            "POZOR: Táto operácia nahradí VŠETKY aktuálne dáta obsahu zálohy.\n\n" +
            "Aplikácia sa po obnovení reštartuje.\n\nPokračovať?",
            "Obnovenie zálohy", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
            return;

        try
        {
            File.Copy(dlg.FileName, _dbPath, overwrite: true);
            MessageBox.Show("Záloha bola obnovená. Aplikácia sa teraz reštartuje.",
                "Obnovenie úspešné", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Application.Restart();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Chyba pri obnovení:\n{ex.Message}", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void AktualizujPoslednaLok()
    {
        lblPoslednaLok.Text = _settings.LastLocalBackup.HasValue
            ? $"Posledná záloha: {_settings.LastLocalBackup:dd.MM.yyyy HH:mm}"
            : "Záloha ešte nebola vytvorená.";
    }

    // ==================== GOOGLE DRIVE ====================

    private async void BtnPripojit_Click(object? sender, EventArgs e)
    {
        if (_drive.JePripojeny)
        {
            if (MessageBox.Show("Odpojiť Google Drive?", "Potvrdenie",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            _drive.Odpojit();
            AktualizujStavDrive();
            lvZalohy.Items.Clear();
            return;
        }

        if (!_settings.MaGooglePrihlasenie)
        {
            MessageBox.Show("Najprv zadajte Google API prihlasenie v Nastaveniach.",
                "Chýbajú údaje", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            BtnNastavenia_Click(sender, e);
            return;
        }

        NastavUI(false, "Pripájam sa...");
        try
        {
            _cts = new CancellationTokenSource(TimeSpan.FromMinutes(2));
            await _drive.PripojitAsync(_settings.GoogleClientId, _settings.GoogleClientSecret,
                _settings.GoogleDriveFolderName, _cts.Token);
            AktualizujStavDrive();
            await NacitajZoznamAsync();
        }
        catch (OperationCanceledException)
        {
            lblProgress.Text = "Pripojenie zrušené.";
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Chyba pri pripájaní:\n{ex.Message}", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            AktualizujStavDrive();
        }
        finally
        {
            NastavUI(true, "");
        }
    }

    private void BtnNastavenia_Click(object? sender, EventArgs e)
    {
        using var dlg = new GoogleNastaveniaDialog(_settings);
        if (dlg.ShowDialog(this) == DialogResult.OK)
        {
            _settings = dlg.Vysledok;
            AktualizujStavDrive();
        }
    }

    private async void BtnNahrat_Click(object? sender, EventArgs e)
    {
        if (!_drive.JePripojeny)
        { MessageBox.Show("Najprv sa pripojte k Google Drive.", "Nie ste pripojení", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

        var zalohaNazov = $"ServisnaKniha_{DateTime.Now:yyyy-MM-dd_HHmm}.db";
        var tmpPath = Path.Combine(Path.GetTempPath(), zalohaNazov);

        NastavUI(false, "Nahrávam zálohu...");
        try
        {
            File.Copy(_dbPath, tmpPath, overwrite: true);
            _cts = new CancellationTokenSource(TimeSpan.FromMinutes(5));
            var progress = new Progress<int>(p =>
            {
                progressBar.Value = Math.Min(p, 100);
                lblProgress.Text = $"Nahrávam... {p}%";
            });
            progressBar.Value = 0;
            progressBar.Visible = true;

            await _drive.NahratZalohuAsync(tmpPath, progress, _cts.Token);

            _settings.LastGoogleBackup = DateTime.Now;
            _settings.Save();
            lblProgress.Text = $"Záloha nahraná: {_settings.LastGoogleBackup:dd.MM.yyyy HH:mm}";
            await NacitajZoznamAsync();
        }
        catch (OperationCanceledException)
        {
            lblProgress.Text = "Nahrávanie zrušené.";
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Chyba pri nahrávaní:\n{ex.Message}", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            lblProgress.Text = "";
        }
        finally
        {
            progressBar.Visible = false;
            NastavUI(true, "");
            if (File.Exists(tmpPath)) File.Delete(tmpPath);
        }
    }

    private async void BtnObnovitZoznam_Click(object? sender, EventArgs e)
    {
        if (!_drive.JePripojeny)
        { MessageBox.Show("Najprv sa pripojte k Google Drive.", "Nie ste pripojení", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
        await NacitajZoznamAsync();
    }

    private async void BtnStahnut_Click(object? sender, EventArgs e)
    {
        if (!_drive.JePripojeny || lvZalohy.SelectedItems.Count == 0) return;
        var item = lvZalohy.SelectedItems[0];
        var fileId = item.Tag?.ToString() ?? "";
        var fileName = item.Text;

        using var dlg = new SaveFileDialog
        {
            Title = "Uložiť stiahnutú zálohu",
            Filter = "Databázový súbor (*.db)|*.db",
            FileName = fileName, DefaultExt = "db"
        };
        if (dlg.ShowDialog() != DialogResult.OK) return;

        NastavUI(false, "Sťahujem zálohu...");
        progressBar.Visible = true;
        progressBar.Value = 0;
        try
        {
            _cts = new CancellationTokenSource(TimeSpan.FromMinutes(5));
            var progress = new Progress<int>(p =>
            {
                progressBar.Value = Math.Min(p, 100);
                lblProgress.Text = $"Sťahujem... {p}%";
            });
            await _drive.StiahnuZalohuAsync(fileId, dlg.FileName, progress, _cts.Token);
            lblProgress.Text = $"Stiahnuté: {dlg.FileName}";

            if (MessageBox.Show(
                "Záloha bola stiahnutá. Chcete ju teraz obnoviť?\n(Nahradí aktuálnu databázu a reštartuje aplikáciu.)",
                "Obnoviť zálohu?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                File.Copy(dlg.FileName, _dbPath, overwrite: true);
                Application.Restart();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Chyba pri sťahovaní:\n{ex.Message}", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            lblProgress.Text = "";
        }
        finally
        {
            progressBar.Visible = false;
            NastavUI(true, "");
        }
    }

    private async void BtnVymazat_Click(object? sender, EventArgs e)
    {
        if (!_drive.JePripojeny || lvZalohy.SelectedItems.Count == 0) return;
        var item = lvZalohy.SelectedItems[0];
        if (MessageBox.Show($"Naozaj vymazať zálohu\n«{item.Text}» z Google Drive?",
            "Potvrdenie", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;

        try
        {
            await _drive.VymazatZalohuAsync(item.Tag?.ToString() ?? "");
            await NacitajZoznamAsync();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Chyba pri mazaní:\n{ex.Message}", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async Task NacitajZoznamAsync()
    {
        if (!_drive.JePripojeny) return;
        NastavUI(false, "Načítavam zoznam...");
        try
        {
            var zalohy = await _drive.GetZoznamZalohAsync();
            lvZalohy.Items.Clear();
            foreach (var z in zalohy)
            {
                var item = new ListViewItem(z.Nazov) { Tag = z.Id };
                item.SubItems.Add(z.DatumVytvorenia == DateTime.MinValue ? "-" : z.DatumVytvorenia.ToString("dd.MM.yyyy HH:mm"));
                item.SubItems.Add(z.VelkostText);
                item.SubItems.Add(z.Popis);
                lvZalohy.Items.Add(item);
            }
            lblProgress.Text = $"Zálohy na Drive: {zalohy.Count}";
        }
        catch (Exception ex)
        {
            lblProgress.Text = $"Chyba: {ex.Message}";
        }
        finally
        {
            NastavUI(true, "");
        }
    }

    private void AktualizujStavDrive()
    {
        if (_drive.JePripojeny)
        {
            lblStavDrive.Text = "✔ Google Drive: Pripojený";
            lblStavDrive.ForeColor = Color.FromArgb(20, 130, 20);
            btnPripojit.Text = "Odpojiť";
            btnPripojit.BackColor = Color.FromArgb(220, 53, 69);
        }
        else
        {
            lblStavDrive.Text = "✘ Google Drive: Nepripojený";
            lblStavDrive.ForeColor = Color.FromArgb(160, 40, 0);
            btnPripojit.Text = "Pripojiť";
            btnPripojit.BackColor = Color.FromArgb(0, 123, 255);
        }
        var pripojeny = _drive.JePripojeny;
        btnNahrat.Enabled = pripojeny;
        btnObnovitZoznam.Enabled = pripojeny;
        btnStahnut.Enabled = pripojeny;
        btnVymazatDrive.Enabled = pripojeny;
    }

    private void NastavUI(bool enabled, string status)
    {
        btnPripojit.Enabled = enabled;
        btnNahrat.Enabled = enabled && _drive.JePripojeny;
        btnObnovitZoznam.Enabled = enabled && _drive.JePripojeny;
        btnStahnut.Enabled = enabled && _drive.JePripojeny;
        btnVymazatDrive.Enabled = enabled && _drive.JePripojeny;
        if (!string.IsNullOrEmpty(status)) lblProgress.Text = status;
    }

    private static Button VytvorBtn(string text, Color bg, int width = 160)
    {
        var b = new Button
        {
            Text = text, Size = new Size(width, 34),
            BackColor = bg, ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand,
            Margin = new Padding(0, 0, 0, 6)
        };
        b.FlatAppearance.BorderSize = 0;
        return b;
    }

    private static Button VytvorBtnMaly(string text, Color bg)
    {
        var b = new Button
        {
            Text = text, AutoSize = true, MinimumSize = new Size(90, 26),
            Padding = new Padding(6, 0, 6, 0),
            BackColor = bg, ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand,
            Margin = new Padding(0, 0, 5, 0)
        };
        b.FlatAppearance.BorderSize = 0;
        return b;
    }
}
