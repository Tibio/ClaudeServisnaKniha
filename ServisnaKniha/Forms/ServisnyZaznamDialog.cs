using ServisnaKniha.Models;
using System.Drawing;
using System.Windows.Forms;

namespace ServisnaKniha.Forms;

public class ServisnyZaznamDialog : Form
{
    private readonly ServisnyZaznam _zaznam;
    private readonly bool _jeNovy;
    private readonly List<Auto> _auta;

    private ComboBox cboAuto = null!;
    private DateTimePicker dtpDatum = null!;
    private NumericUpDown numKM = null!;
    private ComboBox cboTyp = null!;
    private TextBox txtPopis = null!;
    private TextBox txtServis = null!;
    private NumericUpDown numCenaPrace = null!;
    private NumericUpDown numCenaDielov = null!;
    private Label lblCelkova = null!;
    private TextBox txtPoznamka = null!;

    public ServisnyZaznam Vysledok => _zaznam;

    public ServisnyZaznamDialog(List<Auto> auta, ServisnyZaznam? zaznam = null, int? predvyberAutoId = null)
    {
        _auta = auta;
        _jeNovy = zaznam == null;
        _zaznam = zaznam ?? new ServisnyZaznam { DatumServisu = DateTime.Today };
        if (_jeNovy && predvyberAutoId.HasValue) _zaznam.AutoId = predvyberAutoId.Value;
        InitializeComponent();
        NaplnFormular();
    }

    private void InitializeComponent()
    {
        Text = _jeNovy ? "Nový servisný záznam" : "Upraviť servisný záznam";
        Size = new Size(520, 540);
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
            Padding = new Padding(15),
            AutoSize = true
        };
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150));
        panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

        int row = 0;

        panel.Controls.Add(CreateLabel("Vozidlo:"), 0, row);
        cboAuto = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList };
        foreach (var a in _auta) cboAuto.Items.Add(a);
        panel.Controls.Add(cboAuto, 1, row++);

        panel.Controls.Add(CreateLabel("Dátum servisu:"), 0, row);
        dtpDatum = new DateTimePicker { Dock = DockStyle.Fill, Format = DateTimePickerFormat.Short };
        panel.Controls.Add(dtpDatum, 1, row++);

        panel.Controls.Add(CreateLabel("Stav km:"), 0, row);
        numKM = new NumericUpDown
        {
            Dock = DockStyle.Fill, Minimum = 0, Maximum = 9999999,
            ThousandsSeparator = true, DecimalPlaces = 0
        };
        panel.Controls.Add(numKM, 1, row++);

        panel.Controls.Add(CreateLabel("Typ servisu:"), 0, row);
        cboTyp = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList };
        cboTyp.Items.AddRange(TypServisu.Typy);
        panel.Controls.Add(cboTyp, 1, row++);

        panel.Controls.Add(CreateLabel("Popis prác:"), 0, row);
        txtPopis = new TextBox { Dock = DockStyle.Fill, Multiline = true, Height = 70, ScrollBars = ScrollBars.Vertical };
        panel.Controls.Add(txtPopis, 1, row++);

        panel.Controls.Add(CreateLabel("Servis / Dielňa:"), 0, row);
        txtServis = new TextBox { Dock = DockStyle.Fill };
        panel.Controls.Add(txtServis, 1, row++);

        panel.Controls.Add(CreateLabel("Cena práce (€):"), 0, row);
        numCenaPrace = new NumericUpDown
        {
            Dock = DockStyle.Fill, Minimum = 0, Maximum = 99999,
            DecimalPlaces = 2, Increment = 0.01m
        };
        numCenaPrace.ValueChanged += (s, e) => AktualizujCelkovu();
        panel.Controls.Add(numCenaPrace, 1, row++);

        panel.Controls.Add(CreateLabel("Cena dielov (€):"), 0, row);
        numCenaDielov = new NumericUpDown
        {
            Dock = DockStyle.Fill, Minimum = 0, Maximum = 99999,
            DecimalPlaces = 2, Increment = 0.01m
        };
        numCenaDielov.ValueChanged += (s, e) => AktualizujCelkovu();
        panel.Controls.Add(numCenaDielov, 1, row++);

        panel.Controls.Add(CreateLabel("Celková cena:"), 0, row);
        lblCelkova = new Label
        {
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleLeft,
            Font = new Font("Segoe UI", 10f, FontStyle.Bold),
            ForeColor = Color.FromArgb(0, 120, 50)
        };
        panel.Controls.Add(lblCelkova, 1, row++);

        panel.Controls.Add(CreateLabel("Poznámka:"), 0, row);
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

    private static Label CreateLabel(string text) => new()
    {
        Text = text, Dock = DockStyle.Fill,
        TextAlign = ContentAlignment.MiddleRight,
        Padding = new Padding(0, 0, 8, 0)
    };

    private void NaplnFormular()
    {
        if (_zaznam.AutoId > 0)
        {
            var auto = _auta.FirstOrDefault(a => a.Id == _zaznam.AutoId);
            if (auto != null) cboAuto.SelectedItem = auto;
        }
        if (cboAuto.SelectedIndex < 0 && _auta.Count > 0) cboAuto.SelectedIndex = 0;

        dtpDatum.Value = _zaznam.DatumServisu == DateTime.MinValue ? DateTime.Today : _zaznam.DatumServisu;

        var selectedAuto = cboAuto.SelectedItem as Auto;
        numKM.Value = _zaznam.StavKM > 0 ? _zaznam.StavKM : (selectedAuto?.AktualneKM ?? 0);

        cboTyp.SelectedItem = string.IsNullOrEmpty(_zaznam.TypServisu) ? TypServisu.Typy[0] : _zaznam.TypServisu;
        if (cboTyp.SelectedIndex < 0) cboTyp.SelectedIndex = 0;

        txtPopis.Text = _zaznam.Popis;
        txtServis.Text = _zaznam.Servis;
        numCenaPrace.Value = _zaznam.CenaPrace;
        numCenaDielov.Value = _zaznam.CenaDielov;
        txtPoznamka.Text = _zaznam.Poznamka;
        AktualizujCelkovu();
    }

    private void AktualizujCelkovu()
    {
        decimal celk = numCenaPrace.Value + numCenaDielov.Value;
        lblCelkova.Text = $"{celk:N2} €";
    }

    private void BtnUlozit_Click(object? sender, EventArgs e)
    {
        if (cboAuto.SelectedItem is not Auto auto)
        { MessageBox.Show("Vyberte vozidlo.", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
        if (cboTyp.SelectedItem == null)
        { MessageBox.Show("Vyberte typ servisu.", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

        _zaznam.AutoId = auto.Id;
        _zaznam.DatumServisu = dtpDatum.Value;
        _zaznam.StavKM = (int)numKM.Value;
        _zaznam.TypServisu = cboTyp.SelectedItem.ToString()!;
        _zaznam.Popis = txtPopis.Text.Trim();
        _zaznam.Servis = txtServis.Text.Trim();
        _zaznam.CenaPrace = numCenaPrace.Value;
        _zaznam.CenaDielov = numCenaDielov.Value;
        _zaznam.CelkovaCena = numCenaPrace.Value + numCenaDielov.Value;
        _zaznam.Poznamka = txtPoznamka.Text.Trim();

        DialogResult = DialogResult.OK;
        Close();
    }
}
