using ServisnaKniha.Models;
using System.Drawing;
using System.Windows.Forms;

namespace ServisnaKniha.Forms;

public class AutoDialog : Form
{
    private readonly Auto _auto;
    private readonly bool _jeNovy;

    private TextBox txtSPZ = null!;
    private TextBox txtZnacka = null!;
    private TextBox txtModel = null!;
    private NumericUpDown numRok = null!;
    private TextBox txtVIN = null!;
    private ComboBox cboTypMotora = null!;
    private NumericUpDown numObjem = null!;
    private NumericUpDown numVykon = null!;
    private TextBox txtFarba = null!;
    private NumericUpDown numKM = null!;
    private DateTimePicker dtpSTK = null!;
    private DateTimePicker dtpEK = null!;
    private TextBox txtPoznamka = null!;

    public Auto Vysledok => _auto;

    public AutoDialog(Auto? auto = null)
    {
        _jeNovy = auto == null;
        _auto = auto ?? new Auto { DatumSTK = DateTime.Today.AddYear(1), DatumEK = DateTime.Today.AddYear(2) };
        InitializeComponent();
        NaplnFormular();
    }

    private void InitializeComponent()
    {
        Text = _jeNovy ? "Pridať vozidlo" : "Upraviť vozidlo";
        Size = new Size(500, 600);
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Font = new Font("Segoe UI", 9f);
        BackColor = Color.White;

        var panel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 15,
            Padding = new Padding(15),
            AutoSize = true
        };
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150));
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        int row = 0;
        txtSPZ = AddRow(panel, "ŠPZ:", row++);
        txtZnacka = AddRow(panel, "Značka:", row++);
        txtModel = AddRow(panel, "Model:", row++);
        numRok = AddNumRow(panel, "Rok výroby:", row++, 1900, DateTime.Now.Year, DateTime.Now.Year);
        txtVIN = AddRow(panel, "VIN:", row++);

        panel.Controls.Add(CreateLabel("Typ motora:"), 0, row);
        cboTypMotora = new ComboBox { Dock = DockStyle.Fill };
        cboTypMotora.Items.AddRange(["Benzín", "Diesel", "LPG", "CNG", "Hybrid", "Elektrický", "Iné"]);
        cboTypMotora.DropDownStyle = ComboBoxStyle.DropDownList;
        panel.Controls.Add(cboTypMotora, 1, row++);

        numObjem = AddNumRow(panel, "Objem (l):", row++, 0, 10, 0, 1, 1);
        numVykon = AddNumRow(panel, "Výkon (kW):", row++, 0, 1000, 0);
        txtFarba = AddRow(panel, "Farba:", row++);
        numKM = AddNumRow(panel, "Aktuálne km:", row++, 0, 9999999, 0);

        panel.Controls.Add(CreateLabel("STK do:"), 0, row);
        dtpSTK = new DateTimePicker { Dock = DockStyle.Fill, Format = DateTimePickerFormat.Short };
        panel.Controls.Add(dtpSTK, 1, row++);

        panel.Controls.Add(CreateLabel("EK do:"), 0, row);
        dtpEK = new DateTimePicker { Dock = DockStyle.Fill, Format = DateTimePickerFormat.Short };
        panel.Controls.Add(dtpEK, 1, row++);

        panel.Controls.Add(CreateLabel("Poznámka:"), 0, row);
        txtPoznamka = new TextBox { Dock = DockStyle.Fill, Multiline = true, Height = 60, ScrollBars = ScrollBars.Vertical };
        panel.Controls.Add(txtPoznamka, 1, row++);

        var pnlBtn = new FlowLayoutPanel
        {
            Dock = DockStyle.Bottom,
            FlowDirection = FlowDirection.RightToLeft,
            Height = 45,
            Padding = new Padding(10, 5, 10, 5),
            BackColor = Color.FromArgb(245, 245, 245)
        };

        var btnZrusit = new Button { Text = "Zrušiť", Size = new Size(80, 30), DialogResult = DialogResult.Cancel };
        var btnUlozit = new Button
        {
            Text = "Uložiť",
            Size = new Size(80, 30),
            BackColor = Color.FromArgb(0, 120, 215),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat
        };
        btnUlozit.FlatAppearance.BorderSize = 0;
        btnUlozit.Click += BtnUlozit_Click;

        pnlBtn.Controls.AddRange([btnZrusit, btnUlozit]);
        Controls.Add(panel);
        Controls.Add(pnlBtn);
        AcceptButton = btnUlozit;
        CancelButton = btnZrusit;
    }

    private static Label CreateLabel(string text) => new()
    {
        Text = text,
        Dock = DockStyle.Fill,
        TextAlign = ContentAlignment.MiddleRight,
        Padding = new Padding(0, 0, 8, 0)
    };

    private static TextBox AddRow(TableLayoutPanel p, string label, int row)
    {
        p.Controls.Add(CreateLabel(label), 0, row);
        var txt = new TextBox { Dock = DockStyle.Fill };
        p.Controls.Add(txt, 1, row);
        return txt;
    }

    private static NumericUpDown AddNumRow(TableLayoutPanel p, string label, int row,
        decimal min = 0, decimal max = 9999999, decimal val = 0, int decimals = 0, decimal increment = 1)
    {
        p.Controls.Add(CreateLabel(label), 0, row);
        var num = new NumericUpDown
        {
            Dock = DockStyle.Fill,
            Minimum = min, Maximum = max, Value = val,
            DecimalPlaces = decimals, Increment = increment,
            ThousandsSeparator = decimals == 0
        };
        p.Controls.Add(num, 1, row);
        return num;
    }

    private void NaplnFormular()
    {
        txtSPZ.Text = _auto.SPZ;
        txtZnacka.Text = _auto.Znacka;
        txtModel.Text = _auto.Model;
        numRok.Value = _auto.RokVyroby == 0 ? DateTime.Now.Year : _auto.RokVyroby;
        txtVIN.Text = _auto.VIN;
        cboTypMotora.SelectedItem = string.IsNullOrEmpty(_auto.TypMotora) ? "Benzín" : _auto.TypMotora;
        if (cboTypMotora.SelectedIndex < 0) cboTypMotora.SelectedIndex = 0;
        numObjem.Value = (decimal)Math.Min(_auto.ObjemMotora, 10);
        numVykon.Value = _auto.VykonKW;
        txtFarba.Text = _auto.Farba;
        numKM.Value = _auto.AktualneKM;
        dtpSTK.Value = _auto.DatumSTK == DateTime.MinValue ? DateTime.Today.AddYears(1) : _auto.DatumSTK;
        dtpEK.Value = _auto.DatumEK == DateTime.MinValue ? DateTime.Today.AddYears(2) : _auto.DatumEK;
        txtPoznamka.Text = _auto.Poznamka;
    }

    private void BtnUlozit_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtSPZ.Text))
        { MessageBox.Show("Zadajte ŠPZ vozidla.", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
        if (string.IsNullOrWhiteSpace(txtZnacka.Text))
        { MessageBox.Show("Zadajte značku vozidla.", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
        if (string.IsNullOrWhiteSpace(txtModel.Text))
        { MessageBox.Show("Zadajte model vozidla.", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

        _auto.SPZ = txtSPZ.Text.Trim().ToUpper();
        _auto.Znacka = txtZnacka.Text.Trim();
        _auto.Model = txtModel.Text.Trim();
        _auto.RokVyroby = (int)numRok.Value;
        _auto.VIN = txtVIN.Text.Trim().ToUpper();
        _auto.TypMotora = cboTypMotora.SelectedItem?.ToString() ?? "";
        _auto.ObjemMotora = (double)numObjem.Value;
        _auto.VykonKW = (int)numVykon.Value;
        _auto.Farba = txtFarba.Text.Trim();
        _auto.AktualneKM = (int)numKM.Value;
        _auto.DatumSTK = dtpSTK.Value;
        _auto.DatumEK = dtpEK.Value;
        _auto.Poznamka = txtPoznamka.Text.Trim();

        DialogResult = DialogResult.OK;
        Close();
    }
}

internal static class DateTimeExt
{
    public static DateTime AddYear(this DateTime dt, int years) => dt.AddYears(years);
}
