#nullable disable
using System.Drawing;
using System.Windows.Forms;
using ServisnaKniha.Models;

namespace ServisnaKniha.Forms;

partial class ServisnyZaznamDialog
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
        lblAuto = new Label();
        cboAuto = new ComboBox();
        lblDatum = new Label();
        dtpDatum = new DateTimePicker();
        lblKM = new Label();
        numKM = new NumericUpDown();
        lblTyp = new Label();
        cboTyp = new ComboBox();
        lblPopis = new Label();
        txtPopis = new TextBox();
        lblServis = new Label();
        txtServis = new TextBox();
        lblCenaPrace = new Label();
        numCenaPrace = new NumericUpDown();
        lblCenaDielov = new Label();
        numCenaDielov = new NumericUpDown();
        lblCelkovaCena = new Label();
        lblCelkova = new Label();
        lblPoznamka = new Label();
        txtPoznamka = new TextBox();
        btnUlozit = new Button();
        btnZrusit = new Button();

        tableLayoutPanel1.SuspendLayout();
        pnlBtn.SuspendLayout();
        SuspendLayout();

        // tableLayoutPanel1
        tableLayoutPanel1.Dock = DockStyle.Fill;
        tableLayoutPanel1.ColumnCount = 2;
        tableLayoutPanel1.Padding = new Padding(15);
        tableLayoutPanel1.AutoSize = true;
        tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150));
        tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        // Labels
        lblAuto.Text = "Vozidlo:"; lblAuto.Dock = DockStyle.Fill; lblAuto.TextAlign = ContentAlignment.MiddleRight; lblAuto.Padding = new Padding(0, 0, 8, 0);
        lblDatum.Text = "Dátum servisu:"; lblDatum.Dock = DockStyle.Fill; lblDatum.TextAlign = ContentAlignment.MiddleRight; lblDatum.Padding = new Padding(0, 0, 8, 0);
        lblKM.Text = "Stav km:"; lblKM.Dock = DockStyle.Fill; lblKM.TextAlign = ContentAlignment.MiddleRight; lblKM.Padding = new Padding(0, 0, 8, 0);
        lblTyp.Text = "Typ servisu:"; lblTyp.Dock = DockStyle.Fill; lblTyp.TextAlign = ContentAlignment.MiddleRight; lblTyp.Padding = new Padding(0, 0, 8, 0);
        lblPopis.Text = "Popis prác:"; lblPopis.Dock = DockStyle.Fill; lblPopis.TextAlign = ContentAlignment.MiddleRight; lblPopis.Padding = new Padding(0, 0, 8, 0);
        lblServis.Text = "Servis / Dielňa:"; lblServis.Dock = DockStyle.Fill; lblServis.TextAlign = ContentAlignment.MiddleRight; lblServis.Padding = new Padding(0, 0, 8, 0);
        lblCenaPrace.Text = "Cena práce (€):"; lblCenaPrace.Dock = DockStyle.Fill; lblCenaPrace.TextAlign = ContentAlignment.MiddleRight; lblCenaPrace.Padding = new Padding(0, 0, 8, 0);
        lblCenaDielov.Text = "Cena dielov (€):"; lblCenaDielov.Dock = DockStyle.Fill; lblCenaDielov.TextAlign = ContentAlignment.MiddleRight; lblCenaDielov.Padding = new Padding(0, 0, 8, 0);
        lblCelkovaCena.Text = "Celková cena:"; lblCelkovaCena.Dock = DockStyle.Fill; lblCelkovaCena.TextAlign = ContentAlignment.MiddleRight; lblCelkovaCena.Padding = new Padding(0, 0, 8, 0);
        lblPoznamka.Text = "Poznámka:"; lblPoznamka.Dock = DockStyle.Fill; lblPoznamka.TextAlign = ContentAlignment.MiddleRight; lblPoznamka.Padding = new Padding(0, 0, 8, 0);

        // cboAuto
        cboAuto.Dock = DockStyle.Fill;
        cboAuto.DropDownStyle = ComboBoxStyle.DropDownList;

        // dtpDatum
        dtpDatum.Dock = DockStyle.Fill;
        dtpDatum.Format = DateTimePickerFormat.Short;

        // numKM
        numKM.Dock = DockStyle.Fill;
        numKM.Minimum = 0;
        numKM.Maximum = 9999999;
        numKM.ThousandsSeparator = true;
        numKM.DecimalPlaces = 0;

        // cboTyp
        cboTyp.Dock = DockStyle.Fill;
        cboTyp.DropDownStyle = ComboBoxStyle.DropDownList;
        cboTyp.Items.AddRange(TypServisu.Typy);

        // txtPopis
        txtPopis.Dock = DockStyle.Fill;
        txtPopis.Multiline = true;
        txtPopis.Height = 70;
        txtPopis.ScrollBars = ScrollBars.Vertical;

        // txtServis
        txtServis.Dock = DockStyle.Fill;

        // numCenaPrace
        numCenaPrace.Dock = DockStyle.Fill;
        numCenaPrace.Minimum = 0;
        numCenaPrace.Maximum = 99999;
        numCenaPrace.DecimalPlaces = 2;
        numCenaPrace.Increment = 0.01m;
        numCenaPrace.ValueChanged += NumCenaPrace_ValueChanged;

        // numCenaDielov
        numCenaDielov.Dock = DockStyle.Fill;
        numCenaDielov.Minimum = 0;
        numCenaDielov.Maximum = 99999;
        numCenaDielov.DecimalPlaces = 2;
        numCenaDielov.Increment = 0.01m;
        numCenaDielov.ValueChanged += NumCenaDielov_ValueChanged;

        // lblCelkova
        lblCelkova.Dock = DockStyle.Fill;
        lblCelkova.TextAlign = ContentAlignment.MiddleLeft;
        lblCelkova.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
        lblCelkova.ForeColor = Color.FromArgb(0, 120, 50);

        // txtPoznamka
        txtPoznamka.Dock = DockStyle.Fill;
        txtPoznamka.Multiline = true;
        txtPoznamka.Height = 50;

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
        tableLayoutPanel1.Controls.Add(lblAuto, 0, 0);
        tableLayoutPanel1.Controls.Add(cboAuto, 1, 0);
        tableLayoutPanel1.Controls.Add(lblDatum, 0, 1);
        tableLayoutPanel1.Controls.Add(dtpDatum, 1, 1);
        tableLayoutPanel1.Controls.Add(lblKM, 0, 2);
        tableLayoutPanel1.Controls.Add(numKM, 1, 2);
        tableLayoutPanel1.Controls.Add(lblTyp, 0, 3);
        tableLayoutPanel1.Controls.Add(cboTyp, 1, 3);
        tableLayoutPanel1.Controls.Add(lblPopis, 0, 4);
        tableLayoutPanel1.Controls.Add(txtPopis, 1, 4);
        tableLayoutPanel1.Controls.Add(lblServis, 0, 5);
        tableLayoutPanel1.Controls.Add(txtServis, 1, 5);
        tableLayoutPanel1.Controls.Add(lblCenaPrace, 0, 6);
        tableLayoutPanel1.Controls.Add(numCenaPrace, 1, 6);
        tableLayoutPanel1.Controls.Add(lblCenaDielov, 0, 7);
        tableLayoutPanel1.Controls.Add(numCenaDielov, 1, 7);
        tableLayoutPanel1.Controls.Add(lblCelkovaCena, 0, 8);
        tableLayoutPanel1.Controls.Add(lblCelkova, 1, 8);
        tableLayoutPanel1.Controls.Add(lblPoznamka, 0, 9);
        tableLayoutPanel1.Controls.Add(txtPoznamka, 1, 9);

        // Form
        Text = "Servisný záznam";
        Size = new Size(520, 540);
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
    private Label lblAuto, lblDatum, lblKM, lblTyp, lblPopis, lblServis;
    private Label lblCenaPrace, lblCenaDielov, lblCelkovaCena, lblCelkova, lblPoznamka;
    private ComboBox cboAuto, cboTyp;
    private DateTimePicker dtpDatum;
    private NumericUpDown numKM, numCenaPrace, numCenaDielov;
    private TextBox txtPopis, txtServis, txtPoznamka;
    private Button btnUlozit, btnZrusit;
}
