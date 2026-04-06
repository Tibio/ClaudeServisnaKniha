#nullable disable
using System.Drawing;
using System.Windows.Forms;
using ServisnaKniha.Models;

namespace ServisnaKniha.Forms;

partial class AutoDialog
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
        lblSPZ = new Label();
        txtSPZ = new TextBox();
        lblZnacka = new Label();
        cboZnacka = new ComboBox();
        lblModel = new Label();
        txtModel = new TextBox();
        lblRok = new Label();
        numRok = new NumericUpDown();
        lblVIN = new Label();
        txtVIN = new TextBox();
        lblTypMotora = new Label();
        cboTypMotora = new ComboBox();
        lblObjem = new Label();
        numObjem = new NumericUpDown();
        lblVykon = new Label();
        numVykon = new NumericUpDown();
        lblFarba = new Label();
        txtFarba = new TextBox();
        lblKM = new Label();
        numKM = new NumericUpDown();
        lblSTK = new Label();
        dtpSTK = new DateTimePicker();
        lblEK = new Label();
        dtpEK = new DateTimePicker();
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
        tableLayoutPanel1.RowCount = 14;
        tableLayoutPanel1.Padding = new Padding(15);
        tableLayoutPanel1.AutoSize = true;
        tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150));
        tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        // Labels
        lblSPZ.Text = "ŠPZ:"; lblSPZ.Dock = DockStyle.Fill; lblSPZ.TextAlign = ContentAlignment.MiddleRight; lblSPZ.Padding = new Padding(0, 0, 8, 0);
        lblZnacka.Text = "Značka:"; lblZnacka.Dock = DockStyle.Fill; lblZnacka.TextAlign = ContentAlignment.MiddleRight; lblZnacka.Padding = new Padding(0, 0, 8, 0);
        lblModel.Text = "Model:"; lblModel.Dock = DockStyle.Fill; lblModel.TextAlign = ContentAlignment.MiddleRight; lblModel.Padding = new Padding(0, 0, 8, 0);
        lblRok.Text = "Rok výroby:"; lblRok.Dock = DockStyle.Fill; lblRok.TextAlign = ContentAlignment.MiddleRight; lblRok.Padding = new Padding(0, 0, 8, 0);
        lblVIN.Text = "VIN:"; lblVIN.Dock = DockStyle.Fill; lblVIN.TextAlign = ContentAlignment.MiddleRight; lblVIN.Padding = new Padding(0, 0, 8, 0);
        lblTypMotora.Text = "Typ motora:"; lblTypMotora.Dock = DockStyle.Fill; lblTypMotora.TextAlign = ContentAlignment.MiddleRight; lblTypMotora.Padding = new Padding(0, 0, 8, 0);
        lblObjem.Text = "Objem (l):"; lblObjem.Dock = DockStyle.Fill; lblObjem.TextAlign = ContentAlignment.MiddleRight; lblObjem.Padding = new Padding(0, 0, 8, 0);
        lblVykon.Text = "Výkon (kW):"; lblVykon.Dock = DockStyle.Fill; lblVykon.TextAlign = ContentAlignment.MiddleRight; lblVykon.Padding = new Padding(0, 0, 8, 0);
        lblFarba.Text = "Farba:"; lblFarba.Dock = DockStyle.Fill; lblFarba.TextAlign = ContentAlignment.MiddleRight; lblFarba.Padding = new Padding(0, 0, 8, 0);
        lblKM.Text = "Aktuálne km:"; lblKM.Dock = DockStyle.Fill; lblKM.TextAlign = ContentAlignment.MiddleRight; lblKM.Padding = new Padding(0, 0, 8, 0);
        lblSTK.Text = "STK do:"; lblSTK.Dock = DockStyle.Fill; lblSTK.TextAlign = ContentAlignment.MiddleRight; lblSTK.Padding = new Padding(0, 0, 8, 0);
        lblEK.Text = "EK do:"; lblEK.Dock = DockStyle.Fill; lblEK.TextAlign = ContentAlignment.MiddleRight; lblEK.Padding = new Padding(0, 0, 8, 0);
        lblPoznamka.Text = "Poznámka:"; lblPoznamka.Dock = DockStyle.Fill; lblPoznamka.TextAlign = ContentAlignment.MiddleRight; lblPoznamka.Padding = new Padding(0, 0, 8, 0);

        // txtSPZ
        txtSPZ.Dock = DockStyle.Fill;

        // cboZnacka
        cboZnacka.Dock = DockStyle.Fill;
        cboZnacka.DropDownStyle = ComboBoxStyle.DropDown;
        cboZnacka.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
        cboZnacka.AutoCompleteSource = AutoCompleteSource.ListItems;
        cboZnacka.Items.AddRange(ZnackyAut.Zoznam);

        // txtModel
        txtModel.Dock = DockStyle.Fill;

        // numRok
        numRok.Dock = DockStyle.Fill;
        numRok.Minimum = 1900;
        numRok.Maximum = DateTime.Now.Year;
        numRok.Value = DateTime.Now.Year;
        numRok.ThousandsSeparator = false;

        // txtVIN
        txtVIN.Dock = DockStyle.Fill;

        // cboTypMotora
        cboTypMotora.Dock = DockStyle.Fill;
        cboTypMotora.DropDownStyle = ComboBoxStyle.DropDownList;
        cboTypMotora.Items.AddRange(new object[] { "Benzín", "Diesel", "LPG", "CNG", "Hybrid", "Elektrický", "Iné" });

        // numObjem
        numObjem.Dock = DockStyle.Fill;
        numObjem.Minimum = 0;
        numObjem.Maximum = 10;
        numObjem.Value = 0;
        numObjem.DecimalPlaces = 1;
        numObjem.Increment = 0.1m;

        // numVykon
        numVykon.Dock = DockStyle.Fill;
        numVykon.Minimum = 0;
        numVykon.Maximum = 1000;
        numVykon.Value = 0;
        numVykon.ThousandsSeparator = true;

        // txtFarba
        txtFarba.Dock = DockStyle.Fill;

        // numKM
        numKM.Dock = DockStyle.Fill;
        numKM.Minimum = 0;
        numKM.Maximum = 9999999;
        numKM.Value = 0;
        numKM.ThousandsSeparator = true;

        // dtpSTK
        dtpSTK.Dock = DockStyle.Fill;
        dtpSTK.Format = DateTimePickerFormat.Short;

        // dtpEK
        dtpEK.Dock = DockStyle.Fill;
        dtpEK.Format = DateTimePickerFormat.Short;

        // txtPoznamka
        txtPoznamka.Dock = DockStyle.Fill;
        txtPoznamka.Multiline = true;
        txtPoznamka.Height = 60;
        txtPoznamka.ScrollBars = ScrollBars.Vertical;

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

        // tableLayoutPanel1 rows (row indices 0-12)
        tableLayoutPanel1.Controls.Add(lblSPZ, 0, 0);
        tableLayoutPanel1.Controls.Add(txtSPZ, 1, 0);
        tableLayoutPanel1.Controls.Add(lblZnacka, 0, 1);
        tableLayoutPanel1.Controls.Add(cboZnacka, 1, 1);
        tableLayoutPanel1.Controls.Add(lblModel, 0, 2);
        tableLayoutPanel1.Controls.Add(txtModel, 1, 2);
        tableLayoutPanel1.Controls.Add(lblRok, 0, 3);
        tableLayoutPanel1.Controls.Add(numRok, 1, 3);
        tableLayoutPanel1.Controls.Add(lblVIN, 0, 4);
        tableLayoutPanel1.Controls.Add(txtVIN, 1, 4);
        tableLayoutPanel1.Controls.Add(lblTypMotora, 0, 5);
        tableLayoutPanel1.Controls.Add(cboTypMotora, 1, 5);
        tableLayoutPanel1.Controls.Add(lblObjem, 0, 6);
        tableLayoutPanel1.Controls.Add(numObjem, 1, 6);
        tableLayoutPanel1.Controls.Add(lblVykon, 0, 7);
        tableLayoutPanel1.Controls.Add(numVykon, 1, 7);
        tableLayoutPanel1.Controls.Add(lblFarba, 0, 8);
        tableLayoutPanel1.Controls.Add(txtFarba, 1, 8);
        tableLayoutPanel1.Controls.Add(lblKM, 0, 9);
        tableLayoutPanel1.Controls.Add(numKM, 1, 9);
        tableLayoutPanel1.Controls.Add(lblSTK, 0, 10);
        tableLayoutPanel1.Controls.Add(dtpSTK, 1, 10);
        tableLayoutPanel1.Controls.Add(lblEK, 0, 11);
        tableLayoutPanel1.Controls.Add(dtpEK, 1, 11);
        tableLayoutPanel1.Controls.Add(lblPoznamka, 0, 12);
        tableLayoutPanel1.Controls.Add(txtPoznamka, 1, 12);

        // Form
        Text = "Vozidlo";
        Size = new Size(500, 600);
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
    private Label lblSPZ, lblZnacka, lblModel, lblRok, lblVIN, lblTypMotora;
    private Label lblObjem, lblVykon, lblFarba, lblKM, lblSTK, lblEK, lblPoznamka;
    private TextBox txtSPZ, txtModel, txtVIN, txtFarba, txtPoznamka;
    private ComboBox cboZnacka, cboTypMotora;
    private NumericUpDown numRok, numObjem, numVykon, numKM;
    private DateTimePicker dtpSTK, dtpEK;
    private Button btnUlozit, btnZrusit;
}
