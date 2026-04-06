#nullable disable
using System.Drawing;
using System.Windows.Forms;

namespace ServisnaKniha.Forms;

partial class OlejNastaveniaDialog
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null)) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        tableLayoutPanel1 = new TableLayoutPanel();
        pnlBtn = new FlowLayoutPanel();
        lblAutoNazov = new Label();
        lblInfo = new Label();
        chkKM = new CheckBox();
        numKM = new NumericUpDown();
        lblKMPriklad = new Label();
        chkMesiace = new CheckBox();
        numMesiace = new NumericUpDown();
        lblMesiacePriklad = new Label();
        btnUlozit = new Button();
        btnZrusit = new Button();

        tableLayoutPanel1.SuspendLayout();
        pnlBtn.SuspendLayout();
        SuspendLayout();

        // tableLayoutPanel1
        tableLayoutPanel1.Dock = DockStyle.Fill;
        tableLayoutPanel1.ColumnCount = 2;
        tableLayoutPanel1.Padding = new Padding(20, 15, 20, 10);
        tableLayoutPanel1.AutoSize = true;
        tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60));
        tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));

        // lblAutoNazov — overridden in constructor with actual vehicle name
        lblAutoNazov.Text = "Vozidlo:";
        lblAutoNazov.Dock = DockStyle.Fill;
        lblAutoNazov.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
        lblAutoNazov.ForeColor = Color.FromArgb(30, 58, 95);
        lblAutoNazov.Height = 28;

        // lblInfo
        lblInfo.Text =
            "Interval výmeny oleja slúži na výpočet\nďalšej plánovanej výmeny podľa km\nalebo dátumu (mesiace od poslednej výmeny).";
        lblInfo.Dock = DockStyle.Fill;
        lblInfo.ForeColor = Color.FromArgb(80, 80, 80);
        lblInfo.Height = 52;

        // chkKM + numKM
        chkKM.Text = "Interval podľa kilometrov:";
        chkKM.Dock = DockStyle.Fill;
        chkKM.TextAlign = ContentAlignment.MiddleLeft;
        chkKM.CheckedChanged += ChkKM_CheckedChanged;
        numKM.Dock = DockStyle.Fill;
        numKM.Minimum = 1000;
        numKM.Maximum = 100000;
        numKM.Value = 15000;
        numKM.Increment = 1000;
        numKM.ThousandsSeparator = true;
        numKM.Enabled = false;

        // lblKMPriklad
        lblKMPriklad.Text = "Typicky: 10 000, 15 000, 20 000 km";
        lblKMPriklad.Dock = DockStyle.Fill;
        lblKMPriklad.ForeColor = Color.Gray;
        lblKMPriklad.Font = new Font("Segoe UI", 8f);
        lblKMPriklad.Height = 18;

        // chkMesiace + numMesiace
        chkMesiace.Text = "Interval podľa mesiacov:";
        chkMesiace.Dock = DockStyle.Fill;
        chkMesiace.TextAlign = ContentAlignment.MiddleLeft;
        chkMesiace.CheckedChanged += ChkMesiace_CheckedChanged;
        numMesiace.Dock = DockStyle.Fill;
        numMesiace.Minimum = 1;
        numMesiace.Maximum = 60;
        numMesiace.Value = 12;
        numMesiace.Enabled = false;

        // lblMesiacePriklad
        lblMesiacePriklad.Text = "Typicky: 12 mesiacov (1 rok), 6, 24 mesiacov";
        lblMesiacePriklad.Dock = DockStyle.Fill;
        lblMesiacePriklad.ForeColor = Color.Gray;
        lblMesiacePriklad.Font = new Font("Segoe UI", 8f);
        lblMesiacePriklad.Height = 18;

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

        // tableLayoutPanel1 rows
        tableLayoutPanel1.Controls.Add(lblAutoNazov, 0, 0);
        tableLayoutPanel1.SetColumnSpan(lblAutoNazov, 2);
        tableLayoutPanel1.Controls.Add(lblInfo, 0, 1);
        tableLayoutPanel1.SetColumnSpan(lblInfo, 2);
        tableLayoutPanel1.Controls.Add(chkKM, 0, 2);
        tableLayoutPanel1.Controls.Add(numKM, 1, 2);
        tableLayoutPanel1.Controls.Add(lblKMPriklad, 0, 3);
        tableLayoutPanel1.SetColumnSpan(lblKMPriklad, 2);
        tableLayoutPanel1.Controls.Add(chkMesiace, 0, 4);
        tableLayoutPanel1.Controls.Add(numMesiace, 1, 4);
        tableLayoutPanel1.Controls.Add(lblMesiacePriklad, 0, 5);
        tableLayoutPanel1.SetColumnSpan(lblMesiacePriklad, 2);

        // Form
        Text = "Nastavenia intervalu výmeny oleja";
        Size = new Size(430, 320);
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Font = new Font("Segoe UI", 9f);
        BackColor = Color.White;
        AcceptButton = btnUlozit;
        CancelButton = btnZrusit;
        Controls.Add(tableLayoutPanel1);
        Controls.Add(pnlBtn);

        tableLayoutPanel1.ResumeLayout(false);
        pnlBtn.ResumeLayout(false);
        ResumeLayout(false);
        PerformLayout();
    }

    private TableLayoutPanel tableLayoutPanel1;
    private FlowLayoutPanel pnlBtn;
    private Label lblAutoNazov, lblInfo, lblKMPriklad, lblMesiacePriklad;
    private CheckBox chkKM, chkMesiace;
    private NumericUpDown numKM, numMesiace;
    private Button btnUlozit, btnZrusit;
}
