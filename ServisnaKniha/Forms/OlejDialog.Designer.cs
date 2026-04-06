#nullable disable
using System.Drawing;
using System.Windows.Forms;
using ServisnaKniha.Models;

namespace ServisnaKniha.Forms;

partial class OlejDialog
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
        lblSepOlej = new Label();
        lblZnacka = new Label();
        cboZnacka = new ComboBox();
        lblViskozita = new Label();
        cboViskozita = new ComboBox();
        lblObjem = new Label();
        numObjem = new NumericUpDown();
        lblFilter = new Label();
        cboFilter = new ComboBox();
        lblSepCeny = new Label();
        lblCenaOleja = new Label();
        numCenaOleja = new NumericUpDown();
        lblCenaVymeny = new Label();
        numCenaVymeny = new NumericUpDown();
        lblCelkovaCena = new Label();
        lblCelkova = new Label();
        lblServis = new Label();
        txtServis = new TextBox();
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
        tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 155));
        tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        // Standard labels
        lblAuto.Text = "Vozidlo:"; lblAuto.Dock = DockStyle.Fill; lblAuto.TextAlign = ContentAlignment.MiddleRight; lblAuto.Padding = new Padding(0, 0, 8, 0);
        lblDatum.Text = "Dátum výmeny:"; lblDatum.Dock = DockStyle.Fill; lblDatum.TextAlign = ContentAlignment.MiddleRight; lblDatum.Padding = new Padding(0, 0, 8, 0);
        lblKM.Text = "Stav km:"; lblKM.Dock = DockStyle.Fill; lblKM.TextAlign = ContentAlignment.MiddleRight; lblKM.Padding = new Padding(0, 0, 8, 0);
        lblZnacka.Text = "Značka oleja:"; lblZnacka.Dock = DockStyle.Fill; lblZnacka.TextAlign = ContentAlignment.MiddleRight; lblZnacka.Padding = new Padding(0, 0, 8, 0);
        lblViskozita.Text = "Viskozita:"; lblViskozita.Dock = DockStyle.Fill; lblViskozita.TextAlign = ContentAlignment.MiddleRight; lblViskozita.Padding = new Padding(0, 0, 8, 0);
        lblObjem.Text = "Objem (l):"; lblObjem.Dock = DockStyle.Fill; lblObjem.TextAlign = ContentAlignment.MiddleRight; lblObjem.Padding = new Padding(0, 0, 8, 0);
        lblFilter.Text = "Olejový filter:"; lblFilter.Dock = DockStyle.Fill; lblFilter.TextAlign = ContentAlignment.MiddleRight; lblFilter.Padding = new Padding(0, 0, 8, 0);
        lblCenaOleja.Text = "Cena oleja (€):"; lblCenaOleja.Dock = DockStyle.Fill; lblCenaOleja.TextAlign = ContentAlignment.MiddleRight; lblCenaOleja.Padding = new Padding(0, 0, 8, 0);
        lblCenaVymeny.Text = "Cena výmeny (€):"; lblCenaVymeny.Dock = DockStyle.Fill; lblCenaVymeny.TextAlign = ContentAlignment.MiddleRight; lblCenaVymeny.Padding = new Padding(0, 0, 8, 0);
        lblCelkovaCena.Text = "Celková cena:"; lblCelkovaCena.Dock = DockStyle.Fill; lblCelkovaCena.TextAlign = ContentAlignment.MiddleRight; lblCelkovaCena.Padding = new Padding(0, 0, 8, 0);
        lblServis.Text = "Servis / Dielňa:"; lblServis.Dock = DockStyle.Fill; lblServis.TextAlign = ContentAlignment.MiddleRight; lblServis.Padding = new Padding(0, 0, 8, 0);
        lblPoznamka.Text = "Poznámka:"; lblPoznamka.Dock = DockStyle.Fill; lblPoznamka.TextAlign = ContentAlignment.MiddleRight; lblPoznamka.Padding = new Padding(0, 0, 8, 0);

        // Separator labels
        lblSepOlej.Text = "— Olej —";
        lblSepOlej.Dock = DockStyle.Fill;
        lblSepOlej.TextAlign = ContentAlignment.MiddleLeft;
        lblSepOlej.Font = new Font("Segoe UI", 8.5f, FontStyle.Bold);
        lblSepOlej.ForeColor = Color.FromArgb(30, 58, 95);
        lblSepOlej.BackColor = Color.FromArgb(235, 242, 252);
        lblSepOlej.Height = 22;
        lblSepOlej.Padding = new Padding(6, 0, 0, 0);
        lblSepOlej.Margin = new Padding(0, 4, 0, 2);

        lblSepCeny.Text = "— Ceny —";
        lblSepCeny.Dock = DockStyle.Fill;
        lblSepCeny.TextAlign = ContentAlignment.MiddleLeft;
        lblSepCeny.Font = new Font("Segoe UI", 8.5f, FontStyle.Bold);
        lblSepCeny.ForeColor = Color.FromArgb(30, 58, 95);
        lblSepCeny.BackColor = Color.FromArgb(235, 242, 252);
        lblSepCeny.Height = 22;
        lblSepCeny.Padding = new Padding(6, 0, 0, 0);
        lblSepCeny.Margin = new Padding(0, 4, 0, 2);

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

        // cboZnacka
        cboZnacka.Dock = DockStyle.Fill;
        cboZnacka.DropDownStyle = ComboBoxStyle.DropDown;
        cboZnacka.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
        cboZnacka.AutoCompleteSource = AutoCompleteSource.ListItems;
        cboZnacka.Items.AddRange(OlejData.Znacky);

        // cboViskozita
        cboViskozita.Dock = DockStyle.Fill;
        cboViskozita.DropDownStyle = ComboBoxStyle.DropDown;
        cboViskozita.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
        cboViskozita.AutoCompleteSource = AutoCompleteSource.ListItems;
        cboViskozita.Items.AddRange(OlejData.Viskozity);

        // numObjem
        numObjem.Dock = DockStyle.Fill;
        numObjem.Minimum = 0;
        numObjem.Maximum = 30;
        numObjem.DecimalPlaces = 1;
        numObjem.Increment = 0.5m;
        numObjem.Value = 5;

        // cboFilter
        cboFilter.Dock = DockStyle.Fill;
        cboFilter.DropDownStyle = ComboBoxStyle.DropDown;
        cboFilter.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
        cboFilter.AutoCompleteSource = AutoCompleteSource.ListItems;
        cboFilter.Items.AddRange(OlejData.Filtre);

        // numCenaOleja
        numCenaOleja.Dock = DockStyle.Fill;
        numCenaOleja.Minimum = 0;
        numCenaOleja.Maximum = 9999;
        numCenaOleja.DecimalPlaces = 2;
        numCenaOleja.Increment = 0.5m;
        numCenaOleja.ValueChanged += NumCenaOleja_ValueChanged;

        // numCenaVymeny
        numCenaVymeny.Dock = DockStyle.Fill;
        numCenaVymeny.Minimum = 0;
        numCenaVymeny.Maximum = 9999;
        numCenaVymeny.DecimalPlaces = 2;
        numCenaVymeny.Increment = 0.5m;
        numCenaVymeny.ValueChanged += NumCenaVymeny_ValueChanged;

        // lblCelkova
        lblCelkova.Dock = DockStyle.Fill;
        lblCelkova.TextAlign = ContentAlignment.MiddleLeft;
        lblCelkova.Font = new Font("Segoe UI", 11f, FontStyle.Bold);
        lblCelkova.ForeColor = Color.FromArgb(0, 120, 50);

        // txtServis
        txtServis.Dock = DockStyle.Fill;

        // txtPoznamka
        txtPoznamka.Dock = DockStyle.Fill;
        txtPoznamka.Multiline = true;
        txtPoznamka.Height = 45;

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
        // separator Olej spanning 2 columns
        tableLayoutPanel1.Controls.Add(lblSepOlej, 0, 3);
        tableLayoutPanel1.SetColumnSpan(lblSepOlej, 2);
        tableLayoutPanel1.Controls.Add(lblZnacka, 0, 4);
        tableLayoutPanel1.Controls.Add(cboZnacka, 1, 4);
        tableLayoutPanel1.Controls.Add(lblViskozita, 0, 5);
        tableLayoutPanel1.Controls.Add(cboViskozita, 1, 5);
        tableLayoutPanel1.Controls.Add(lblObjem, 0, 6);
        tableLayoutPanel1.Controls.Add(numObjem, 1, 6);
        tableLayoutPanel1.Controls.Add(lblFilter, 0, 7);
        tableLayoutPanel1.Controls.Add(cboFilter, 1, 7);
        // separator Ceny spanning 2 columns
        tableLayoutPanel1.Controls.Add(lblSepCeny, 0, 8);
        tableLayoutPanel1.SetColumnSpan(lblSepCeny, 2);
        tableLayoutPanel1.Controls.Add(lblCenaOleja, 0, 9);
        tableLayoutPanel1.Controls.Add(numCenaOleja, 1, 9);
        tableLayoutPanel1.Controls.Add(lblCenaVymeny, 0, 10);
        tableLayoutPanel1.Controls.Add(numCenaVymeny, 1, 10);
        tableLayoutPanel1.Controls.Add(lblCelkovaCena, 0, 11);
        tableLayoutPanel1.Controls.Add(lblCelkova, 1, 11);
        tableLayoutPanel1.Controls.Add(lblServis, 0, 12);
        tableLayoutPanel1.Controls.Add(txtServis, 1, 12);
        tableLayoutPanel1.Controls.Add(lblPoznamka, 0, 13);
        tableLayoutPanel1.Controls.Add(txtPoznamka, 1, 13);

        // Form
        Text = "Výmena oleja";
        Size = new Size(520, 560);
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
    private Label lblAuto, lblDatum, lblKM, lblSepOlej, lblZnacka, lblViskozita;
    private Label lblObjem, lblFilter, lblSepCeny, lblCenaOleja, lblCenaVymeny;
    private Label lblCelkovaCena, lblCelkova, lblServis, lblPoznamka;
    private ComboBox cboAuto, cboZnacka, cboViskozita, cboFilter;
    private DateTimePicker dtpDatum;
    private NumericUpDown numKM, numObjem, numCenaOleja, numCenaVymeny;
    private TextBox txtServis, txtPoznamka;
    private Button btnUlozit, btnZrusit;
}
