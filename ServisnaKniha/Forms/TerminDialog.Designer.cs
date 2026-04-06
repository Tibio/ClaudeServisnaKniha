#nullable disable
using System.Drawing;
using System.Windows.Forms;

namespace ServisnaKniha.Forms;

partial class TerminDialog
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
        lblPopis = new Label();
        txtPopis = new TextBox();
        chkDatum = new CheckBox();
        dtpDatum = new DateTimePicker();
        chkKM = new CheckBox();
        numKM = new NumericUpDown();
        chkOpakDni = new CheckBox();
        numOpakDni = new NumericUpDown();
        chkOpakKM = new CheckBox();
        numOpakKM = new NumericUpDown();
        lblStav = new Label();
        chkAktivny = new CheckBox();
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

        // Labels
        lblAuto.Text = "Vozidlo:"; lblAuto.Dock = DockStyle.Fill; lblAuto.TextAlign = ContentAlignment.MiddleRight; lblAuto.Padding = new Padding(0, 0, 8, 0);
        lblNazov.Text = "Názov termínu:"; lblNazov.Dock = DockStyle.Fill; lblNazov.TextAlign = ContentAlignment.MiddleRight; lblNazov.Padding = new Padding(0, 0, 8, 0);
        lblPopis.Text = "Popis:"; lblPopis.Dock = DockStyle.Fill; lblPopis.TextAlign = ContentAlignment.MiddleRight; lblPopis.Padding = new Padding(0, 0, 8, 0);
        lblStav.Text = "Stav:"; lblStav.Dock = DockStyle.Fill; lblStav.TextAlign = ContentAlignment.MiddleRight; lblStav.Padding = new Padding(0, 0, 8, 0);
        lblPoznamka.Text = "Poznámka:"; lblPoznamka.Dock = DockStyle.Fill; lblPoznamka.TextAlign = ContentAlignment.MiddleRight; lblPoznamka.Padding = new Padding(0, 0, 8, 0);

        // cboAuto
        cboAuto.Dock = DockStyle.Fill;
        cboAuto.DropDownStyle = ComboBoxStyle.DropDownList;

        // txtNazov
        txtNazov.Dock = DockStyle.Fill;

        // txtPopis
        txtPopis.Dock = DockStyle.Fill;
        txtPopis.Multiline = true;
        txtPopis.Height = 50;

        // chkDatum + dtpDatum
        chkDatum.Text = "Termín dátumom:";
        chkDatum.Dock = DockStyle.Fill;
        chkDatum.TextAlign = ContentAlignment.MiddleRight;
        chkDatum.CheckedChanged += ChkDatum_CheckedChanged;
        dtpDatum.Dock = DockStyle.Fill;
        dtpDatum.Format = DateTimePickerFormat.Short;
        dtpDatum.Enabled = false;

        // chkKM + numKM
        chkKM.Text = "Termín km:";
        chkKM.Dock = DockStyle.Fill;
        chkKM.TextAlign = ContentAlignment.MiddleRight;
        chkKM.CheckedChanged += ChkKM_CheckedChanged;
        numKM.Dock = DockStyle.Fill;
        numKM.Minimum = 0;
        numKM.Maximum = 9999999;
        numKM.ThousandsSeparator = true;
        numKM.Enabled = false;

        // chkOpakDni + numOpakDni
        chkOpakDni.Text = "Opakovať každých X dní:";
        chkOpakDni.Dock = DockStyle.Fill;
        chkOpakDni.TextAlign = ContentAlignment.MiddleRight;
        chkOpakDni.CheckedChanged += ChkOpakDni_CheckedChanged;
        numOpakDni.Dock = DockStyle.Fill;
        numOpakDni.Minimum = 1;
        numOpakDni.Maximum = 3650;
        numOpakDni.Value = 365;
        numOpakDni.Enabled = false;

        // chkOpakKM + numOpakKM
        chkOpakKM.Text = "Opakovať každých X km:";
        chkOpakKM.Dock = DockStyle.Fill;
        chkOpakKM.TextAlign = ContentAlignment.MiddleRight;
        chkOpakKM.CheckedChanged += ChkOpakKM_CheckedChanged;
        numOpakKM.Dock = DockStyle.Fill;
        numOpakKM.Minimum = 100;
        numOpakKM.Maximum = 999999;
        numOpakKM.Value = 15000;
        numOpakKM.ThousandsSeparator = true;
        numOpakKM.Enabled = false;

        // chkAktivny
        chkAktivny.Text = "Termín je aktívny";
        chkAktivny.Dock = DockStyle.Fill;
        chkAktivny.Checked = true;

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
        tableLayoutPanel1.Controls.Add(lblPopis, 0, 2);
        tableLayoutPanel1.Controls.Add(txtPopis, 1, 2);
        tableLayoutPanel1.Controls.Add(chkDatum, 0, 3);
        tableLayoutPanel1.Controls.Add(dtpDatum, 1, 3);
        tableLayoutPanel1.Controls.Add(chkKM, 0, 4);
        tableLayoutPanel1.Controls.Add(numKM, 1, 4);
        tableLayoutPanel1.Controls.Add(chkOpakDni, 0, 5);
        tableLayoutPanel1.Controls.Add(numOpakDni, 1, 5);
        tableLayoutPanel1.Controls.Add(chkOpakKM, 0, 6);
        tableLayoutPanel1.Controls.Add(numOpakKM, 1, 6);
        tableLayoutPanel1.Controls.Add(lblStav, 0, 7);
        tableLayoutPanel1.Controls.Add(chkAktivny, 1, 7);
        tableLayoutPanel1.Controls.Add(lblPoznamka, 0, 8);
        tableLayoutPanel1.Controls.Add(txtPoznamka, 1, 8);

        // Form
        Text = "Servisný termín";
        Size = new Size(480, 530);
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
    private Label lblAuto, lblNazov, lblPopis, lblStav, lblPoznamka;
    private ComboBox cboAuto;
    private TextBox txtNazov, txtPopis, txtPoznamka;
    private CheckBox chkDatum, chkKM, chkOpakDni, chkOpakKM, chkAktivny;
    private DateTimePicker dtpDatum;
    private NumericUpDown numKM, numOpakDni, numOpakKM;
    private Button btnUlozit, btnZrusit;
}
