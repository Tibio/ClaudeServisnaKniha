#nullable disable
using System.Drawing;
using System.Windows.Forms;

namespace ServisnaKniha.Forms;

partial class GoogleNastaveniaDialog
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null)) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        outerLayout = new TableLayoutPanel();
        lblNadpis = new Label();
        pnlInfo = new Panel();
        lblInfoText = new Label();
        gridLayout = new TableLayoutPanel();
        lblClientId = new Label();
        txtClientId = new TextBox();
        lblClientSecret = new Label();
        txtClientSecret = new TextBox();
        lblFolderName = new Label();
        txtFolderName = new TextBox();
        lblWarning = new Label();
        pnlBtn = new FlowLayoutPanel();
        btnUlozit = new Button();
        btnZrusit = new Button();

        outerLayout.SuspendLayout();
        pnlInfo.SuspendLayout();
        gridLayout.SuspendLayout();
        pnlBtn.SuspendLayout();
        SuspendLayout();

        // outerLayout
        outerLayout.Dock = DockStyle.Fill;
        outerLayout.ColumnCount = 1;
        outerLayout.Padding = new Padding(20, 15, 20, 10);
        outerLayout.AutoSize = true;

        // lblNadpis
        lblNadpis.Text = "Google Drive — nastavenia pripojenia";
        lblNadpis.Font = new Font("Segoe UI", 12f, FontStyle.Bold);
        lblNadpis.ForeColor = Color.FromArgb(30, 58, 95);
        lblNadpis.Dock = DockStyle.Fill;
        lblNadpis.Height = 30;

        // pnlInfo
        pnlInfo.Dock = DockStyle.Fill;
        pnlInfo.BackColor = Color.FromArgb(232, 245, 255);
        pnlInfo.Padding = new Padding(12);
        pnlInfo.Height = 170;
        pnlInfo.BorderStyle = BorderStyle.FixedSingle;

        // lblInfoText
        lblInfoText.Text =
            "Ako získať Client ID a Client Secret:\r\n\r\n" +
            "1. Otvorte console.cloud.google.com a vytvorte nový projekt.\r\n" +
            "2. V menu vyberte: APIs & Services → Library → vyhľadajte\r\n" +
            "   \"Google Drive API\" a kliknite Enable.\r\n" +
            "3. APIs & Services → Credentials → Create Credentials\r\n" +
            "   → OAuth 2.0 Client ID → Desktop app.\r\n" +
            "4. Stiahnite JSON alebo skopírujte Client ID a Client Secret.\r\n" +
            "5. Pri prvom pripojení sa otvorí prehliadač na overenie účtu.";
        lblInfoText.Dock = DockStyle.Fill;
        lblInfoText.ForeColor = Color.FromArgb(30, 60, 100);
        pnlInfo.Controls.Add(lblInfoText);

        // gridLayout
        gridLayout.Dock = DockStyle.Fill;
        gridLayout.ColumnCount = 2;
        gridLayout.Padding = new Padding(0, 10, 0, 0);
        gridLayout.AutoSize = true;
        gridLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 140));
        gridLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        // Labels in grid
        lblClientId.Text = "Client ID:"; lblClientId.Dock = DockStyle.Fill; lblClientId.TextAlign = ContentAlignment.MiddleRight; lblClientId.Padding = new Padding(0, 0, 8, 0);
        lblClientSecret.Text = "Client Secret:"; lblClientSecret.Dock = DockStyle.Fill; lblClientSecret.TextAlign = ContentAlignment.MiddleRight; lblClientSecret.Padding = new Padding(0, 0, 8, 0);
        lblFolderName.Text = "Priečinok na Drive:"; lblFolderName.Dock = DockStyle.Fill; lblFolderName.TextAlign = ContentAlignment.MiddleRight; lblFolderName.Padding = new Padding(0, 0, 8, 0);

        // TextBoxes
        txtClientId.Dock = DockStyle.Fill;
        txtClientSecret.Dock = DockStyle.Fill;
        txtClientSecret.UseSystemPasswordChar = false;
        txtFolderName.Dock = DockStyle.Fill;

        // gridLayout rows
        gridLayout.Controls.Add(lblClientId, 0, 0);
        gridLayout.Controls.Add(txtClientId, 1, 0);
        gridLayout.Controls.Add(lblClientSecret, 0, 1);
        gridLayout.Controls.Add(txtClientSecret, 1, 1);
        gridLayout.Controls.Add(lblFolderName, 0, 2);
        gridLayout.Controls.Add(txtFolderName, 1, 2);

        // lblWarning
        lblWarning.Text = "⚠ Prihlasovacie údaje sa ukladajú lokálne len na tomto PC.";
        lblWarning.ForeColor = Color.FromArgb(150, 80, 0);
        lblWarning.Dock = DockStyle.Fill;
        lblWarning.Height = 24;

        // btnUlozit
        btnUlozit.Text = "Uložiť";
        btnUlozit.Size = new Size(80, 30);
        btnUlozit.BackColor = Color.FromArgb(0, 120, 215);
        btnUlozit.ForeColor = Color.White;
        btnUlozit.FlatStyle = FlatStyle.Flat;
        btnUlozit.FlatAppearance.BorderSize = 0;
        btnUlozit.Click += BtnUlozit_Click;

        // btnZrusit
        btnZrusit.Text = "Zrušiť";
        btnZrusit.Size = new Size(80, 30);
        btnZrusit.DialogResult = DialogResult.Cancel;

        // pnlBtn
        pnlBtn.Dock = DockStyle.Bottom;
        pnlBtn.FlowDirection = FlowDirection.RightToLeft;
        pnlBtn.Height = 45;
        pnlBtn.Padding = new Padding(10, 5, 10, 5);
        pnlBtn.BackColor = Color.FromArgb(245, 245, 245);
        pnlBtn.Controls.AddRange(new Control[] { btnZrusit, btnUlozit });

        // outerLayout rows
        outerLayout.Controls.Add(lblNadpis, 0, 0);
        outerLayout.Controls.Add(pnlInfo, 0, 1);
        outerLayout.Controls.Add(gridLayout, 0, 2);
        outerLayout.Controls.Add(lblWarning, 0, 3);

        // Form
        Text = "Nastavenia Google Drive";
        Size = new Size(600, 480);
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Font = new Font("Segoe UI", 9f);
        BackColor = Color.White;
        AcceptButton = btnUlozit;
        CancelButton = btnZrusit;
        Controls.Add(outerLayout);
        Controls.Add(pnlBtn);

        outerLayout.ResumeLayout(false);
        pnlInfo.ResumeLayout(false);
        gridLayout.ResumeLayout(false);
        pnlBtn.ResumeLayout(false);
        ResumeLayout(false);
        PerformLayout();
    }

    private TableLayoutPanel outerLayout, gridLayout;
    private FlowLayoutPanel pnlBtn;
    private Panel pnlInfo;
    private Label lblNadpis, lblInfoText, lblClientId, lblClientSecret, lblFolderName, lblWarning;
    private TextBox txtClientId, txtClientSecret, txtFolderName;
    private Button btnUlozit, btnZrusit;
}
