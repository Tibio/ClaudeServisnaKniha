#nullable disable
using System.Drawing;
using System.Windows.Forms;
using ServisnaKniha.Models;

namespace ServisnaKniha.Forms;

partial class NakladDialog
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
        lblKategoria = new Label();
        cboKategoria = new ComboBox();
        lblPopis = new Label();
        txtPopis = new TextBox();
        lblSuma = new Label();
        numSuma = new NumericUpDown();
        lblDoklad = new Label();
        txtDoklad = new TextBox();
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
        lblDatum.Text = "Dátum:"; lblDatum.Dock = DockStyle.Fill; lblDatum.TextAlign = ContentAlignment.MiddleRight; lblDatum.Padding = new Padding(0, 0, 8, 0);
        lblKategoria.Text = "Kategória:"; lblKategoria.Dock = DockStyle.Fill; lblKategoria.TextAlign = ContentAlignment.MiddleRight; lblKategoria.Padding = new Padding(0, 0, 8, 0);
        lblPopis.Text = "Popis:"; lblPopis.Dock = DockStyle.Fill; lblPopis.TextAlign = ContentAlignment.MiddleRight; lblPopis.Padding = new Padding(0, 0, 8, 0);
        lblSuma.Text = "Suma (€):"; lblSuma.Dock = DockStyle.Fill; lblSuma.TextAlign = ContentAlignment.MiddleRight; lblSuma.Padding = new Padding(0, 0, 8, 0);
        lblDoklad.Text = "Číslo dokladu:"; lblDoklad.Dock = DockStyle.Fill; lblDoklad.TextAlign = ContentAlignment.MiddleRight; lblDoklad.Padding = new Padding(0, 0, 8, 0);
        lblPoznamka.Text = "Poznámka:"; lblPoznamka.Dock = DockStyle.Fill; lblPoznamka.TextAlign = ContentAlignment.MiddleRight; lblPoznamka.Padding = new Padding(0, 0, 8, 0);

        // cboAuto
        cboAuto.Dock = DockStyle.Fill;
        cboAuto.DropDownStyle = ComboBoxStyle.DropDownList;

        // dtpDatum
        dtpDatum.Dock = DockStyle.Fill;
        dtpDatum.Format = DateTimePickerFormat.Short;

        // cboKategoria
        cboKategoria.Dock = DockStyle.Fill;
        cboKategoria.DropDownStyle = ComboBoxStyle.DropDownList;
        cboKategoria.Items.AddRange(KategoriaNakladu.Kategorie);

        // txtPopis
        txtPopis.Dock = DockStyle.Fill;
        txtPopis.Multiline = true;
        txtPopis.Height = 60;

        // numSuma
        numSuma.Dock = DockStyle.Fill;
        numSuma.Minimum = 0;
        numSuma.Maximum = 999999;
        numSuma.DecimalPlaces = 2;
        numSuma.Increment = 0.01m;
        numSuma.Font = new Font("Segoe UI", 10f, FontStyle.Bold);

        // txtDoklad
        txtDoklad.Dock = DockStyle.Fill;

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
        tableLayoutPanel1.Controls.Add(lblKategoria, 0, 2);
        tableLayoutPanel1.Controls.Add(cboKategoria, 1, 2);
        tableLayoutPanel1.Controls.Add(lblPopis, 0, 3);
        tableLayoutPanel1.Controls.Add(txtPopis, 1, 3);
        tableLayoutPanel1.Controls.Add(lblSuma, 0, 4);
        tableLayoutPanel1.Controls.Add(numSuma, 1, 4);
        tableLayoutPanel1.Controls.Add(lblDoklad, 0, 5);
        tableLayoutPanel1.Controls.Add(txtDoklad, 1, 5);
        tableLayoutPanel1.Controls.Add(lblPoznamka, 0, 6);
        tableLayoutPanel1.Controls.Add(txtPoznamka, 1, 6);

        // Form
        Text = "Náklad";
        Size = new Size(460, 420);
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
    private Label lblAuto, lblDatum, lblKategoria, lblPopis, lblSuma, lblDoklad, lblPoznamka;
    private ComboBox cboAuto, cboKategoria;
    private DateTimePicker dtpDatum;
    private NumericUpDown numSuma;
    private TextBox txtPopis, txtDoklad, txtPoznamka;
    private Button btnUlozit, btnZrusit;
}
