using ServisnaKniha.Models;
using System.Drawing;
using System.Windows.Forms;

namespace ServisnaKniha.Forms;

public class OEMDielDialog : Form
{
    private readonly OEMDiel _diel;
    private readonly bool _jeNovy;
    private readonly List<Auto> _auta;

    private ComboBox cboAuto = null!;
    private TextBox txtNazov = null!;
    private TextBox txtOEM = null!;
    private TextBox txtVyrobca = null!;
    private ComboBox cboKategoria = null!;
    private NumericUpDown numPocet = null!;
    private NumericUpDown numCena = null!;
    private Label lblCelkova = null!;
    private DateTimePicker dtpDatum = null!;
    private NumericUpDown numKM = null!;
    private TextBox txtPoznamka = null!;

    public OEMDiel Vysledok => _diel;

    public OEMDielDialog(List<Auto> auta, OEMDiel? diel = null, int? predvyberAutoId = null)
    {
        _auta = auta;
        _jeNovy = diel == null;
        _diel = diel ?? new OEMDiel { DatumMontaze = DateTime.Today };
        if (_jeNovy && predvyberAutoId.HasValue) _diel.AutoId = predvyberAutoId.Value;
        InitializeComponent();
        NaplnFormular();
    }

    private void InitializeComponent()
    {
        Text = _jeNovy ? "Pridať OEM diel" : "Upraviť OEM diel";
        Size = new Size(500, 520);
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
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 160));
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        int row = 0;

        panel.Controls.Add(Lbl("Vozidlo:"), 0, row);
        cboAuto = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList };
        foreach (var a in _auta) cboAuto.Items.Add(a);
        panel.Controls.Add(cboAuto, 1, row++);

        panel.Controls.Add(Lbl("Názov dielu:"), 0, row);
        txtNazov = new TextBox { Dock = DockStyle.Fill };
        panel.Controls.Add(txtNazov, 1, row++);

        panel.Controls.Add(Lbl("OEM číslo:"), 0, row);
        txtOEM = new TextBox { Dock = DockStyle.Fill };
        panel.Controls.Add(txtOEM, 1, row++);

        panel.Controls.Add(Lbl("Výrobca:"), 0, row);
        txtVyrobca = new TextBox { Dock = DockStyle.Fill };
        panel.Controls.Add(txtVyrobca, 1, row++);

        panel.Controls.Add(Lbl("Kategória:"), 0, row);
        cboKategoria = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList };
        cboKategoria.Items.AddRange(KategoriaDielu.Kategorie);
        panel.Controls.Add(cboKategoria, 1, row++);

        panel.Controls.Add(Lbl("Počet (ks):"), 0, row);
        numPocet = new NumericUpDown { Dock = DockStyle.Fill, Minimum = 1, Maximum = 999, Value = 1 };
        numPocet.ValueChanged += (s, e) => AktualizujCelkovu();
        panel.Controls.Add(numPocet, 1, row++);

        panel.Controls.Add(Lbl("Cena / ks (€):"), 0, row);
        numCena = new NumericUpDown
        {
            Dock = DockStyle.Fill, Minimum = 0, Maximum = 99999,
            DecimalPlaces = 2, Increment = 0.01m
        };
        numCena.ValueChanged += (s, e) => AktualizujCelkovu();
        panel.Controls.Add(numCena, 1, row++);

        panel.Controls.Add(Lbl("Celková cena:"), 0, row);
        lblCelkova = new Label
        {
            Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft,
            Font = new Font("Segoe UI", 10f, FontStyle.Bold),
            ForeColor = Color.FromArgb(0, 120, 50)
        };
        panel.Controls.Add(lblCelkova, 1, row++);

        panel.Controls.Add(Lbl("Dátum montáže:"), 0, row);
        dtpDatum = new DateTimePicker { Dock = DockStyle.Fill, Format = DateTimePickerFormat.Short };
        panel.Controls.Add(dtpDatum, 1, row++);

        panel.Controls.Add(Lbl("KM pri montáži:"), 0, row);
        numKM = new NumericUpDown
        {
            Dock = DockStyle.Fill, Minimum = 0, Maximum = 9999999,
            ThousandsSeparator = true
        };
        panel.Controls.Add(numKM, 1, row++);

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
        if (_diel.AutoId > 0)
        {
            var auto = _auta.FirstOrDefault(a => a.Id == _diel.AutoId);
            if (auto != null) cboAuto.SelectedItem = auto;
        }
        if (cboAuto.SelectedIndex < 0 && _auta.Count > 0) cboAuto.SelectedIndex = 0;

        txtNazov.Text = _diel.NazovDielu;
        txtOEM.Text = _diel.OEMCislo;
        txtVyrobca.Text = _diel.Vyrobca;

        cboKategoria.SelectedItem = string.IsNullOrEmpty(_diel.Kategoria) ? KategoriaDielu.Kategorie[0] : _diel.Kategoria;
        if (cboKategoria.SelectedIndex < 0) cboKategoria.SelectedIndex = 0;

        numPocet.Value = _diel.Pocet;
        numCena.Value = _diel.CenaKus;
        dtpDatum.Value = _diel.DatumMontaze == DateTime.MinValue ? DateTime.Today : _diel.DatumMontaze;
        numKM.Value = _diel.KMPriMontazi;
        txtPoznamka.Text = _diel.Poznamka;
        AktualizujCelkovu();
    }

    private void AktualizujCelkovu() =>
        lblCelkova.Text = $"{numPocet.Value * numCena.Value:N2} €";

    private void BtnUlozit_Click(object? sender, EventArgs e)
    {
        if (cboAuto.SelectedItem is not Auto auto)
        { MessageBox.Show("Vyberte vozidlo.", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
        if (string.IsNullOrWhiteSpace(txtNazov.Text))
        { MessageBox.Show("Zadajte názov dielu.", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

        _diel.AutoId = auto.Id;
        _diel.NazovDielu = txtNazov.Text.Trim();
        _diel.OEMCislo = txtOEM.Text.Trim();
        _diel.Vyrobca = txtVyrobca.Text.Trim();
        _diel.Kategoria = cboKategoria.SelectedItem?.ToString() ?? "";
        _diel.Pocet = (int)numPocet.Value;
        _diel.CenaKus = numCena.Value;
        _diel.DatumMontaze = dtpDatum.Value;
        _diel.KMPriMontazi = (int)numKM.Value;
        _diel.Poznamka = txtPoznamka.Text.Trim();

        DialogResult = DialogResult.OK;
        Close();
    }
}
