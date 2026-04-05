using ServisnaKniha.Models;
using System.Drawing;
using System.Windows.Forms;

namespace ServisnaKniha.Forms;

public class OlejDialog : Form
{
    private readonly OlejZaznam _zaznam;
    private readonly bool _jeNovy;
    private readonly List<Auto> _auta;

    private ComboBox cboAuto = null!;
    private DateTimePicker dtpDatum = null!;
    private NumericUpDown numKM = null!;
    private ComboBox cboZnacka = null!;
    private ComboBox cboViskozita = null!;
    private NumericUpDown numObjem = null!;
    private ComboBox cboFilter = null!;
    private NumericUpDown numCenaOleja = null!;
    private NumericUpDown numCenaVymeny = null!;
    private Label lblCelkova = null!;
    private TextBox txtServis = null!;
    private TextBox txtPoznamka = null!;

    public OlejZaznam Vysledok => _zaznam;

    public OlejDialog(List<Auto> auta, OlejZaznam? zaznam = null, int? predvyberAutoId = null)
    {
        _auta = auta;
        _jeNovy = zaznam == null;
        _zaznam = zaznam ?? new OlejZaznam { DatumVymeny = DateTime.Today };
        if (_jeNovy && predvyberAutoId.HasValue) _zaznam.AutoId = predvyberAutoId.Value;
        InitializeComponent();
        NaplnFormular();
    }

    private void InitializeComponent()
    {
        Text = _jeNovy ? "Nová výmena oleja" : "Upraviť výmenu oleja";
        Size = new Size(520, 560);
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
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 155));
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        int row = 0;

        panel.Controls.Add(Lbl("Vozidlo:"), 0, row);
        cboAuto = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList };
        foreach (var a in _auta) cboAuto.Items.Add(a);
        panel.Controls.Add(cboAuto, 1, row++);

        panel.Controls.Add(Lbl("Dátum výmeny:"), 0, row);
        dtpDatum = new DateTimePicker { Dock = DockStyle.Fill, Format = DateTimePickerFormat.Short };
        panel.Controls.Add(dtpDatum, 1, row++);

        panel.Controls.Add(Lbl("Stav km:"), 0, row);
        numKM = new NumericUpDown
        {
            Dock = DockStyle.Fill, Minimum = 0, Maximum = 9999999,
            ThousandsSeparator = true, DecimalPlaces = 0
        };
        panel.Controls.Add(numKM, 1, row++);

        // Oddeľovač — olej
        panel.Controls.Add(Separator("Olej"), 0, row);
        panel.SetColumnSpan(panel.Controls[panel.Controls.Count - 1], 2);
        row++;

        panel.Controls.Add(Lbl("Značka oleja:"), 0, row);
        cboZnacka = new ComboBox
        {
            Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDown,
            AutoCompleteMode = AutoCompleteMode.SuggestAppend,
            AutoCompleteSource = AutoCompleteSource.ListItems
        };
        cboZnacka.Items.AddRange(OlejData.Znacky);
        panel.Controls.Add(cboZnacka, 1, row++);

        panel.Controls.Add(Lbl("Viskozita:"), 0, row);
        cboViskozita = new ComboBox
        {
            Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDown,
            AutoCompleteMode = AutoCompleteMode.SuggestAppend,
            AutoCompleteSource = AutoCompleteSource.ListItems
        };
        cboViskozita.Items.AddRange(OlejData.Viskozity);
        panel.Controls.Add(cboViskozita, 1, row++);

        panel.Controls.Add(Lbl("Objem (l):"), 0, row);
        numObjem = new NumericUpDown
        {
            Dock = DockStyle.Fill, Minimum = 0, Maximum = 30,
            DecimalPlaces = 1, Increment = 0.5m, Value = 5
        };
        panel.Controls.Add(numObjem, 1, row++);

        panel.Controls.Add(Lbl("Olejový filter:"), 0, row);
        cboFilter = new ComboBox
        {
            Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDown,
            AutoCompleteMode = AutoCompleteMode.SuggestAppend,
            AutoCompleteSource = AutoCompleteSource.ListItems
        };
        cboFilter.Items.AddRange(OlejData.Filtre);
        panel.Controls.Add(cboFilter, 1, row++);

        // Oddeľovač — ceny
        panel.Controls.Add(Separator("Ceny"), 0, row);
        panel.SetColumnSpan(panel.Controls[panel.Controls.Count - 1], 2);
        row++;

        panel.Controls.Add(Lbl("Cena oleja (€):"), 0, row);
        numCenaOleja = new NumericUpDown
        {
            Dock = DockStyle.Fill, Minimum = 0, Maximum = 9999,
            DecimalPlaces = 2, Increment = 0.5m
        };
        numCenaOleja.ValueChanged += (s, e) => AktualizujCelkovu();
        panel.Controls.Add(numCenaOleja, 1, row++);

        panel.Controls.Add(Lbl("Cena výmeny (€):"), 0, row);
        numCenaVymeny = new NumericUpDown
        {
            Dock = DockStyle.Fill, Minimum = 0, Maximum = 9999,
            DecimalPlaces = 2, Increment = 0.5m
        };
        numCenaVymeny.ValueChanged += (s, e) => AktualizujCelkovu();
        panel.Controls.Add(numCenaVymeny, 1, row++);

        panel.Controls.Add(Lbl("Celková cena:"), 0, row);
        lblCelkova = new Label
        {
            Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft,
            Font = new Font("Segoe UI", 11f, FontStyle.Bold),
            ForeColor = Color.FromArgb(0, 120, 50)
        };
        panel.Controls.Add(lblCelkova, 1, row++);

        panel.Controls.Add(Lbl("Servis / Dielňa:"), 0, row);
        txtServis = new TextBox { Dock = DockStyle.Fill };
        panel.Controls.Add(txtServis, 1, row++);

        panel.Controls.Add(Lbl("Poznámka:"), 0, row);
        txtPoznamka = new TextBox { Dock = DockStyle.Fill, Multiline = true, Height = 45 };
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

    private static Label Separator(string text) => new()
    {
        Text = $"— {text} —",
        Dock = DockStyle.Fill,
        TextAlign = ContentAlignment.MiddleLeft,
        Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
        ForeColor = Color.FromArgb(30, 58, 95),
        BackColor = Color.FromArgb(235, 242, 252),
        Height = 22,
        Padding = new Padding(6, 0, 0, 0),
        Margin = new Padding(0, 4, 0, 2)
    };

    private void NaplnFormular()
    {
        if (_zaznam.AutoId > 0)
        {
            var auto = _auta.FirstOrDefault(a => a.Id == _zaznam.AutoId);
            if (auto != null) cboAuto.SelectedItem = auto;
        }
        if (cboAuto.SelectedIndex < 0 && _auta.Count > 0) cboAuto.SelectedIndex = 0;

        dtpDatum.Value = _zaznam.DatumVymeny == DateTime.MinValue ? DateTime.Today : _zaznam.DatumVymeny;

        var auto2 = cboAuto.SelectedItem as Auto;
        numKM.Value = _zaznam.StavKM > 0 ? _zaznam.StavKM : (auto2?.AktualneKM ?? 0);

        cboZnacka.Text = _zaznam.ZnackaOleja;
        cboViskozita.Text = _zaznam.ViskozitaOleja;
        numObjem.Value = _zaznam.ObjemOleja > 0 ? (decimal)_zaznam.ObjemOleja : 5;
        cboFilter.Text = _zaznam.FilterOleja;
        numCenaOleja.Value = _zaznam.CenaOleja;
        numCenaVymeny.Value = _zaznam.CenaVymeny;
        txtServis.Text = _zaznam.Servis;
        txtPoznamka.Text = _zaznam.Poznamka;
        AktualizujCelkovu();
    }

    private void AktualizujCelkovu() =>
        lblCelkova.Text = $"{numCenaOleja.Value + numCenaVymeny.Value:N2} €";

    private void BtnUlozit_Click(object? sender, EventArgs e)
    {
        if (cboAuto.SelectedItem is not Auto auto)
        { MessageBox.Show("Vyberte vozidlo.", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

        _zaznam.AutoId = auto.Id;
        _zaznam.DatumVymeny = dtpDatum.Value;
        _zaznam.StavKM = (int)numKM.Value;
        _zaznam.ZnackaOleja = cboZnacka.Text.Trim();
        _zaznam.ViskozitaOleja = cboViskozita.Text.Trim();
        _zaznam.ObjemOleja = (double)numObjem.Value;
        _zaznam.FilterOleja = cboFilter.Text.Trim();
        _zaznam.CenaOleja = numCenaOleja.Value;
        _zaznam.CenaVymeny = numCenaVymeny.Value;
        _zaznam.Servis = txtServis.Text.Trim();
        _zaznam.Poznamka = txtPoznamka.Text.Trim();

        DialogResult = DialogResult.OK;
        Close();
    }
}
