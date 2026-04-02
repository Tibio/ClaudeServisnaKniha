using ServisnaKniha.Database;
using ServisnaKniha.Forms;
using System.Windows.Forms;

namespace ServisnaKniha;

static class Program
{
    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        ApplicationConfiguration.Initialize();

        // Databáza v AppData/Local/ServisnaKniha
        var appDataDir = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "ServisnaKniha");
        Directory.CreateDirectory(appDataDir);
        var dbPath = Path.Combine(appDataDir, "servisna_kniha.db");

        var db = new DatabaseManager(dbPath);
        Application.Run(new HlavnyFormular(db, dbPath));
    }
}
