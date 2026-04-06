#nullable disable
using System.Drawing;
using System.Windows.Forms;
using ServisnaKniha.Models;

namespace ServisnaKniha.Forms;

partial class OEMDielDialog
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
        lblNazov = new Label();
        txtNazov = new TextBox();
        lblOEM = new Label();
        txtOEM = new TextBox();
        lblVyrobca = new Label();
        txtVyrobca = new TextBox();
        lblKategoria = new Label();
        cboKategoria = new ComboBox();
        lblPocet = new Label();
        numPocet = new NumericUpDown();
        lblCena = new Label();
        numCena = new NumericUpDown();
        lblCelkovaCena = new Label();
        lblCelkova = new Label();
        lblDatum = new Label();
        dtpDatum = new DateTimePicker();
        lblKM = new Label();
        numKM = new NumericUpDown();
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
        tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 160));
        tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        // Labels
        lblAuto.Text = "Vozidlo:"; lblAuto.Dock = DockStyle.Fill; lblAuto.TextAlign = ContentAlignment.MiddleRight; lblAuto.Padding = new Padding(0, 0, 8, 0);
        lblNazov.Text = "Názov dielu:"; lblNazov.Dock = DockStyle.Fill; lblNazov.TextAlign = ContentAlignment.MiddleRight; lblNazov.Padding = new Padding(0, 0, 8, 0);
        lblOEM.Text = "OEM číslo:"; lblOEM.Dock = DockStyle.Fill; lblOEM.TextAlign = ContentAlignment.MiddleRight; lblOEM.Padding = new Padding(0, 0, 8, 0);
        lblVyrobca.Text = "Výrobca:"; lblVyrobca.Dock = DockStyle.Fill; lblVyrobca.TextAlign = ContentAlignment.MiddleRight; lblVyrobca.Padding = new Padding(0, 0, 8, 0);
        lblKategoria.Text = "Kategória:"; lblKategoria.Dock = DockStyle.Fill; lblKategoria.TextAlign = ContentAlignment.MiddleRight; lblKategoria.Padding = new Padding(0, 0, 8, 0);
        lblPocet.Text = "Počet (ks):"; lblPocet.Dock = DockStyle.Fill; lblPocet.TextAlign = ContentAlignment.MiddleRight; lblPocet.Padding = new Padding(0, 0, 8, 0);
        lblCena.Text = "Cena / ks (€):"; lblCena.Dock = DockStyle.Fill; lblCena.TextAlign = ContentAlignment.MiddleRight; lblCena.Padding = new Padding(0, 0, 8, 0);
        lblCelkovaCena.Text = "Celková cena:"; lblCelkovaCena.Dock = DockStyle.Fill; lblCelkovaCena.TextAlign = ContentAlignment.MiddleRight; lblCelkovaCena.Padding = new Padding(0, 0, 8, 0);
        lblDatum.Text = "Dátum montáže:"; lblDatum.Dock = DockStyle.Fill; lblDatum.TextAlign = ContentAlignment.MiddleRight; lblDatum.Padding = new Padding(0, 0, 8, 0);
        lblKM.Text = "KM pri montáži:"; lblKM.Dock = DockStyle.Fill; lblKM.TextAlign = ContentAlignment.MiddleRight; lblKM.Padding = new Padding(0, 0, 8, 0);
        lblPoznamka.Text = "Poznámka:"; lblPoznamka.Dock = DockStyle.Fill; lblPoznamka.TextAlign = ContentAlignment.MiddleRight; lblPoznamka.Padding = new Padding(0, 0, 8, 0);

        // cboAuto
        cboAuto.Dock = DockStyle.Fill;
        cboAuto.DropDownStyle = ComboBoxStyle.DropDownList;

        // txtNazov
        txtNazov.Dock = DockStyle.Fill;

        // txtOEM
        txtOEM.Dock = DockStyle.Fill;

        // txtVyrobca
        txtVyrobca.Dock = DockStyle.Fill;

        // cboKategoria
        cboKategoria.Dock = DockStyle.Fill;
        cboKategoria.DropDownStyle = ComboBoxStyle.DropDownList;
        cboKategoria.Items.AddRange(KategoriaDielu.Kategorie);

        // numPocet
        numPocet.Dock = DockStyle.Fill;
        numPocet.Minimum = 1;
        numPocet.Maximum = 999;
        numPocet.Value = 1;
        numPocet.ValueChanged += NumPocet_ValueChanged;

        // numCena
        numCena.Dock = DockStyle.Fill;
        numCena.Minimum = 0;
        numCena.Maximum = 99999;
        numCena.DecimalPlaces = 2;
        numCena.Increment = 0.01m;
        numCena.ValueChanged += NumCena_ValueChanged;

        // lblCelkova
        lblCelkova.Dock = DockStyle.Fill;
        lblCelkova.TextAlign = ContentAlignment.MiddleLeft;
        lblCelkova.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
        lblCelkova.ForeColor = Color.FromArgb(0, 120, 50);

        // dtpDatum
        dtpDatum.Dock = DockStyle.Fill;
        dtpDatum.Format = DateTimePickerFormat.Short;

        // numKM
        numKM.Dock = DockStyle.Fill;
        numKM.Minimum = 0;
        numKM.Maximum = 9999999;
        numKM.ThousandsSeparator = true;

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
        tableLayoutPanel1.Controls.Add(lblNazov, 0, 1);
        tableLayoutPanel1.Controls.Add(txtNazov, 1, 1);
        tableLayoutPanel1.Controls.Add(lblOEM, 0, 2);
        tableLayoutPanel1.Controls.Add(txtOEM, 1, 2);
        tableLayoutPanel1.Controls.Add(lblVyrobca, 0, 3);
        tableLayoutPanel1.Controls.Add(txtVyrobca, 1, 3);
        tableLayoutPanel1.Controls.Add(lblKategoria, 0, 4);
        tableLayoutPanel1.Controls.Add(cboKategoria, 1, 4);
        tableLayoutPanel1.Controls.Add(lblPocet, 0, 5);
        tableLayoutPanel1.Controls.Add(numPocet, 1, 5);
        tableLayoutPanel1.Controls.Add(lblCena, 0, 6);
        tableLayoutPanel1.Controls.Add(numCena, 1, 6);
        tableLayoutPanel1.Controls.Add(lblCelkovaCena, 0, 7);
        tableLayoutPanel1.Controls.Add(lblCelkova, 1, 7);
        tableLayoutPanel1.Controls.Add(lblDatum, 0, 8);
        tableLayoutPanel1.Controls.Add(dtpDatum, 1, 8);
        tableLayoutPanel1.Controls.Add(lblKM, 0, 9);
        tableLayoutPanel1.Controls.Add(numKM, 1, 9);
        tableLayoutPanel1.Controls.Add(lblPoznamka, 0, 10);
        tableLayoutPanel1.Controls.Add(txtPoznamka, 1, 10);

        // Form
        Text = "OEM diel";
        Size = new Size(500, 520);
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
    private Label lblAuto, lblNazov, lblOEM, lblVyrobca, lblKategoria, lblPocet;
    private Label lblCena, lblCelkovaCena, lblCelkova, lblDatum, lblKM, lblPoznamka;
    private ComboBox cboAuto, cboKategoria;
    private TextBox txtNazov, txtOEM, txtVyrobca, txtPoznamka;
    private NumericUpDown numPocet, numCena, numKM;
    private DateTimePicker dtpDatum;
    private Button btnUlozit, btnZrusit;
}
