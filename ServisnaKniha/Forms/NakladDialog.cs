using ServisnaKniha.Models;
using System.Drawing;
using System.Windows.Forms;

namespace ServisnaKniha.Forms;

public class NakladDialog : Form
{
    private readonly Naklad _naklad;
    private readonly bool _jeNovy;
    private readonly List<Auto> _auta;

    private ComboBox cboAuto = null!;
    private DateTimePicker dtpDatum = null!;
    private ComboBox cboKategoria = null!;
    private TextBox txtPopis = null!;
    private NumericUpDown numSuma = null!;
    private TextBox txtDoklad = null!;
    private TextBox txtPoznamka = null!;

    public Naklad Vysledok => _naklad;

    public NakladDialog(List<Auto> auta, Naklad? naklad = null, int? predvyberAutoId = null)
    {
        _auta = auta;
        _jeNovy = naklad == null;
        _naklad = naklad ?? new Naklad { Datum = DateTime.Today, Mena = "EUR" };
        if (_jeNovy && predvyberAutoId.HasValue) _naklad.AutoId = predvyberAutoId.Value;
        InitializeComponent();
        NaplnFormular();
    }

    private void InitializeComponent()
    {
        Text = _jeNovy ? "Pridať náklad" : "Upraviť náklad";
        Size = new Size(460, 420);
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false; MinimizeBox = false;
        Font = new Font("Segoe UI", 9f);
        BackColor = Color.White;

        var panel = new TableLayoutPanel
        {
            Dock = DockStyle.Fill, ColumnCount = 2,
            Padding = new Padding(15), AutoSize = true
        };
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150));
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        int row = 0;

        panel.Controls.Add(Lbl("Vozidlo:"), 0, row);
        cboAuto = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList };
        foreach (var a in _auta) cboAuto.Items.Add(a);
        panel.Controls.Add(cboAuto, 1, row++);

        panel.Controls.Add(Lbl("Dátum:"), 0, row);
        dtpDatum = new DateTimePicker { Dock = DockStyle.Fill, Format = DateTimePickerFormat.Short };
        panel.Controls.Add(dtpDatum, 1, row++);

        panel.Controls.Add(Lbl("Kategória:"), 0, row);
        cboKategoria = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList };
        cboKategoria.Items.AddRange(KategoriaNakladu.Kategorie);
        panel.Controls.Add(cboKategoria, 1, row++);

        panel.Controls.Add(Lbl("Popis:"), 0, row);
        txtPopis = new TextBox { Dock = DockStyle.Fill, Multiline = true, Height = 60 };
        panel.Controls.Add(txtPopis, 1, row++);

        panel.Controls.Add(Lbl("Suma (€):"), 0, row);
        numSuma = new NumericUpDown
        {
            Dock = DockStyle.Fill, Minimum = 0, Maximum = 999999,
            DecimalPlaces = 2, Increment = 0.01m,
            Font = new Font("Segoe UI", 10f, FontStyle.Bold)
        };
        panel.Controls.Add(numSuma, 1, row++);

        panel.Controls.Add(Lbl("Číslo dokladu:"), 0, row);
        txtDoklad = new TextBox { Dock = DockStyle.Fill };
        panel.Controls.Add(txtDoklad, 1, row++);

        panel.Controls.Add(Lbl("Poznámka:"), 0, row);
        txtPoznamka = new TextBox { Dock = DockStyle.Fill, Multiline = true, Height = 50 };
        panel.Controls.Add(txtPoznamka, 1, row);

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

        Controls.Add(panel);
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
        if (_naklad.AutoId > 0)
        {
            var auto = _auta.FirstOrDefault(a => a.Id == _naklad.AutoId);
            if (auto != null) cboAuto.SelectedItem = auto;
        }
        if (cboAuto.SelectedIndex < 0 && _auta.Count > 0) cboAuto.SelectedIndex = 0;

        dtpDatum.Value = _naklad.Datum == DateTime.MinValue ? DateTime.Today : _naklad.Datum;
        cboKategoria.SelectedItem = string.IsNullOrEmpty(_naklad.Kategoria) ? KategoriaNakladu.Kategorie[0] : _naklad.Kategoria;
        if (cboKategoria.SelectedIndex < 0) cboKategoria.SelectedIndex = 0;
        txtPopis.Text = _naklad.Popis;
        numSuma.Value = _naklad.Suma;
        txtDoklad.Text = _naklad.Doklad;
        txtPoznamka.Text = _naklad.Poznamka;
    }

    private void BtnUlozit_Click(object? sender, EventArgs e)
    {
        if (cboAuto.SelectedItem is not Auto auto)
        { MessageBox.Show("Vyberte vozidlo.", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
        if (numSuma.Value <= 0)
        { MessageBox.Show("Zadajte sumu väčšiu ako 0.", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

        _naklad.AutoId = auto.Id;
        _naklad.Datum = dtpDatum.Value;
        _naklad.Kategoria = cboKategoria.SelectedItem?.ToString() ?? "";
        _naklad.Popis = txtPopis.Text.Trim();
        _naklad.Suma = numSuma.Value;
        _naklad.Doklad = txtDoklad.Text.Trim();
        _naklad.Poznamka = txtPoznamka.Text.Trim();

        DialogResult = DialogResult.OK;
        Close();
    }
}
