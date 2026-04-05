using ServisnaKniha.Models;
using System.Drawing;
using System.Windows.Forms;

namespace ServisnaKniha.Forms;

public class GoogleNastaveniaDialog : Form
{
    private TextBox txtClientId = null!;
    private TextBox txtClientSecret = null!;
    private TextBox txtFolderName = null!;

    public AppSettings Vysledok { get; }

    public GoogleNastaveniaDialog(AppSettings settings)
    {
        Vysledok = settings;
        InitializeComponent();
        NaplnFormular();
    }

    private void InitializeComponent()
    {
        Text = "Nastavenia Google Drive";
        Size = new Size(600, 480);
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false; MinimizeBox = false;
        Font = new Font("Segoe UI", 9f);
        BackColor = Color.White;

        var layout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill, ColumnCount = 1,
            Padding = new Padding(20, 15, 20, 10), AutoSize = true
        };

        // Nadpis
        layout.Controls.Add(new Label
        {
            Text = "Google Drive — nastavenia pripojenia",
            Font = new Font("Segoe UI", 12f, FontStyle.Bold),
            ForeColor = Color.FromArgb(30, 58, 95),
            Dock = DockStyle.Fill, Height = 30
        });

        // Inštrukcie
        var pnlInfo = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.FromArgb(232, 245, 255),
            Padding = new Padding(12),
            Height = 170,
            BorderStyle = BorderStyle.FixedSingle
        };
        var lblInfo = new Label
        {
            Text =
                "Ako získať Client ID a Client Secret:\r\n\r\n" +
                "1. Otvorte console.cloud.google.com a vytvorte nový projekt.\r\n" +
                "2. V menu vyberte: APIs & Services → Library → vyhľadajte\r\n" +
                "   \"Google Drive API\" a kliknite Enable.\r\n" +
                "3. APIs & Services → Credentials → Create Credentials\r\n" +
                "   → OAuth 2.0 Client ID → Desktop app.\r\n" +
                "4. Stiahnite JSON alebo skopírujte Client ID a Client Secret.\r\n" +
                "5. Pri prvom pripojení sa otvorí prehliadač na overenie účtu.",
            Dock = DockStyle.Fill,
            ForeColor = Color.FromArgb(30, 60, 100)
        };
        pnlInfo.Controls.Add(lblInfo);
        layout.Controls.Add(pnlInfo);

        // Formulár
        var grid = new TableLayoutPanel
        {
            Dock = DockStyle.Fill, ColumnCount = 2,
            Padding = new Padding(0, 10, 0, 0), AutoSize = true
        };
        grid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 140));
        grid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        grid.Controls.Add(Lbl("Client ID:"), 0, 0);
        txtClientId = new TextBox { Dock = DockStyle.Fill };
        grid.Controls.Add(txtClientId, 1, 0);

        grid.Controls.Add(Lbl("Client Secret:"), 0, 1);
        txtClientSecret = new TextBox { Dock = DockStyle.Fill, UseSystemPasswordChar = false };
        grid.Controls.Add(txtClientSecret, 1, 1);

        grid.Controls.Add(Lbl("Priečinok na Drive:"), 0, 2);
        txtFolderName = new TextBox { Dock = DockStyle.Fill };
        grid.Controls.Add(txtFolderName, 1, 2);

        layout.Controls.Add(grid);

        // Upozornenie
        layout.Controls.Add(new Label
        {
            Text = "⚠ Prihlasovacie údaje sa ukladajú lokálne len na tomto PC.",
            ForeColor = Color.FromArgb(150, 80, 0),
            Dock = DockStyle.Fill, Height = 24
        });

        // Tlačidlá
        var pnlBtn = new FlowLayoutPanel
        {
            Dock = DockStyle.Bottom, FlowDirection = FlowDirection.RightToLeft,
            Height = 45, Padding = new Padding(10, 5, 10, 5),
            BackColor = Color.FromArgb(245, 245, 245)
        };
        var btnZrusit = new Button { Text = "Zrušiť", Size = new Size(80, 30), DialogResult = DialogResult.Cancel };
        var btnUlozit = new Button
        {
            Text = "Uložiť", Size = new Size(80, 30),
            BackColor = Color.FromArgb(0, 120, 215), ForeColor = Color.White, FlatStyle = FlatStyle.Flat
        };
        btnUlozit.FlatAppearance.BorderSize = 0;
        btnUlozit.Click += BtnUlozit_Click;
        pnlBtn.Controls.AddRange([btnZrusit, btnUlozit]);

        Controls.Add(layout);
        Controls.Add(pnlBtn);
        AcceptButton = btnUlozit;
        CancelButton = btnZrusit;
    }

    private static Label Lbl(string text) => new()
    {
        Text = text, Dock = DockStyle.Fill,
        TextAlign = ContentAlignment.MiddleRight, Padding = new Padding(0, 0, 8, 0)
    };

    private void NaplnFormular()
    {
        txtClientId.Text = Vysledok.GoogleClientId;
        txtClientSecret.Text = Vysledok.GoogleClientSecret;
        txtFolderName.Text = Vysledok.GoogleDriveFolderName;
    }

    private void BtnUlozit_Click(object? sender, EventArgs e)
    {
        Vysledok.GoogleClientId = txtClientId.Text.Trim();
        Vysledok.GoogleClientSecret = txtClientSecret.Text.Trim();
        Vysledok.GoogleDriveFolderName = string.IsNullOrWhiteSpace(txtFolderName.Text)
            ? "ServisnaKniha_Zalohy" : txtFolderName.Text.Trim();
        Vysledok.Save();
        DialogResult = DialogResult.OK;
        Close();
    }
}
