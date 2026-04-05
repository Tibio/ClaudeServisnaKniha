using ServisnaKniha.Models;
using System.Drawing;
using System.Windows.Forms;

namespace ServisnaKniha.Forms;

public class TerminDialog : Form
{
    private readonly ServisnyTermin _termin;
    private readonly bool _jeNovy;
    private readonly List<Auto> _auta;

    private ComboBox cboAuto = null!;
    private TextBox txtNazov = null!;
    private TextBox txtPopis = null!;
    private CheckBox chkDatum = null!;
    private DateTimePicker dtpDatum = null!;
    private CheckBox chkKM = null!;
    private NumericUpDown numKM = null!;
    private CheckBox chkOpakDni = null!;
    private NumericUpDown numOpakDni = null!;
    private CheckBox chkOpakKM = null!;
    private NumericUpDown numOpakKM = null!;
    private CheckBox chkAktivny = null!;
    private TextBox txtPoznamka = null!;

    public ServisnyTermin Vysledok => _termin;

    public TerminDialog(List<Auto> auta, ServisnyTermin? termin = null, int? predvyberAutoId = null)
    {
        _auta = auta;
        _jeNovy = termin == null;
        _termin = termin ?? new ServisnyTermin();
        if (_jeNovy && predvyberAutoId.HasValue) _termin.AutoId = predvyberAutoId.Value;
        InitializeComponent();
        NaplnFormular();
    }

    private void InitializeComponent()
    {
        Text = _jeNovy ? "Nový servisný termín" : "Upraviť servisný termín";
        Size = new Size(480, 530);
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

        panel.Controls.Add(Lbl("Názov termínu:"), 0, row);
        txtNazov = new TextBox { Dock = DockStyle.Fill };
        panel.Controls.Add(txtNazov, 1, row++);

        panel.Controls.Add(Lbl("Popis:"), 0, row);
        txtPopis = new TextBox { Dock = DockStyle.Fill, Multiline = true, Height = 50 };
        panel.Controls.Add(txtPopis, 1, row++);

        // Dátum termínu
        chkDatum = new CheckBox { Text = "Termín dátumom:", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleRight };
        chkDatum.CheckedChanged += (s, e) => dtpDatum.Enabled = chkDatum.Checked;
        panel.Controls.Add(chkDatum, 0, row);
        dtpDatum = new DateTimePicker { Dock = DockStyle.Fill, Format = DateTimePickerFormat.Short, Enabled = false };
        panel.Controls.Add(dtpDatum, 1, row++);

        // KM termín
        chkKM = new CheckBox { Text = "Termín km:", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleRight };
        chkKM.CheckedChanged += (s, e) => numKM.Enabled = chkKM.Checked;
        panel.Controls.Add(chkKM, 0, row);
        numKM = new NumericUpDown
        {
            Dock = DockStyle.Fill, Minimum = 0, Maximum = 9999999,
            ThousandsSeparator = true, Enabled = false
        };
        panel.Controls.Add(numKM, 1, row++);

        // Opakovanie dni
        chkOpakDni = new CheckBox { Text = "Opakovať každých X dní:", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleRight };
        chkOpakDni.CheckedChanged += (s, e) => numOpakDni.Enabled = chkOpakDni.Checked;
        panel.Controls.Add(chkOpakDni, 0, row);
        numOpakDni = new NumericUpDown { Dock = DockStyle.Fill, Minimum = 1, Maximum = 3650, Value = 365, Enabled = false };
        panel.Controls.Add(numOpakDni, 1, row++);

        // Opakovanie KM
        chkOpakKM = new CheckBox { Text = "Opakovať každých X km:", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleRight };
        chkOpakKM.CheckedChanged += (s, e) => numOpakKM.Enabled = chkOpakKM.Checked;
        panel.Controls.Add(chkOpakKM, 0, row);
        numOpakKM = new NumericUpDown
        {
            Dock = DockStyle.Fill, Minimum = 100, Maximum = 999999,
            Value = 15000, ThousandsSeparator = true, Enabled = false
        };
        panel.Controls.Add(numOpakKM, 1, row++);

        panel.Controls.Add(Lbl("Stav:"), 0, row);
        chkAktivny = new CheckBox { Text = "Termín je aktívny", Dock = DockStyle.Fill, Checked = true };
        panel.Controls.Add(chkAktivny, 1, row++);

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
        if (_termin.AutoId > 0)
        {
            var auto = _auta.FirstOrDefault(a => a.Id == _termin.AutoId);
            if (auto != null) cboAuto.SelectedItem = auto;
        }
        if (cboAuto.SelectedIndex < 0 && _auta.Count > 0) cboAuto.SelectedIndex = 0;

        txtNazov.Text = _termin.Nazov;
        txtPopis.Text = _termin.Popis;

        if (_termin.DatumTerminu.HasValue)
        {
            chkDatum.Checked = true;
            dtpDatum.Value = _termin.DatumTerminu.Value;
        }
        if (_termin.KMTerminu.HasValue)
        {
            chkKM.Checked = true;
            numKM.Value = _termin.KMTerminu.Value;
        }
        if (_termin.OpakovanieDni.HasValue)
        {
            chkOpakDni.Checked = true;
            numOpakDni.Value = _termin.OpakovanieDni.Value;
        }
        if (_termin.OpakovaniKM.HasValue)
        {
            chkOpakKM.Checked = true;
            numOpakKM.Value = _termin.OpakovaniKM.Value;
        }
        chkAktivny.Checked = _termin.JeAktivny;
        txtPoznamka.Text = _termin.Poznamka;
    }

    private void BtnUlozit_Click(object? sender, EventArgs e)
    {
        if (cboAuto.SelectedItem is not Auto auto)
        { MessageBox.Show("Vyberte vozidlo.", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
        if (string.IsNullOrWhiteSpace(txtNazov.Text))
        { MessageBox.Show("Zadajte názov termínu.", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
        if (!chkDatum.Checked && !chkKM.Checked)
        { MessageBox.Show("Zadajte aspoň termín dátumom alebo km.", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

        _termin.AutoId = auto.Id;
        _termin.Nazov = txtNazov.Text.Trim();
        _termin.Popis = txtPopis.Text.Trim();
        _termin.DatumTerminu = chkDatum.Checked ? dtpDatum.Value : null;
        _termin.KMTerminu = chkKM.Checked ? (int)numKM.Value : null;
        _termin.OpakovanieDni = chkOpakDni.Checked ? (int)numOpakDni.Value : null;
        _termin.OpakovaniKM = chkOpakKM.Checked ? (int)numOpakKM.Value : null;
        _termin.JeAktivny = chkAktivny.Checked;
        _termin.Poznamka = txtPoznamka.Text.Trim();

        DialogResult = DialogResult.OK;
        Close();
    }
}
