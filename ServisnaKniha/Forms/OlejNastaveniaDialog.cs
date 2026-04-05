using ServisnaKniha.Models;
using System.Drawing;
using System.Windows.Forms;

namespace ServisnaKniha.Forms;

public class OlejNastaveniaDialog : Form
{
    private readonly OlejNastavenia _nastavenia;
    private readonly string _autoNazov;

    private CheckBox chkKM = null!;
    private NumericUpDown numKM = null!;
    private CheckBox chkMesiace = null!;
    private NumericUpDown numMesiace = null!;

    public OlejNastavenia Vysledok => _nastavenia;

    public OlejNastaveniaDialog(OlejNastavenia nastavenia, string autoNazov)
    {
        _nastavenia = nastavenia;
        _autoNazov = autoNazov;
        InitializeComponent();
        NaplnFormular();
    }

    private void InitializeComponent()
    {
        Text = "Nastavenia intervalu výmeny oleja";
        Size = new Size(430, 320);
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false; MinimizeBox = false;
        Font = new Font("Segoe UI", 9f);
        BackColor = Color.White;

        var layout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill, ColumnCount = 2,
            Padding = new Padding(20, 15, 20, 10), AutoSize = true
        };
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60));
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));

        // Hlavička
        var lblAuto = new Label
        {
            Text = $"Vozidlo: {_autoNazov}",
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI", 10f, FontStyle.Bold),
            ForeColor = Color.FromArgb(30, 58, 95),
            Height = 28
        };
        layout.Controls.Add(lblAuto, 0, 0);
        layout.SetColumnSpan(lblAuto, 2);

        // Info
        var lblInfo = new Label
        {
            Text = "Interval výmeny oleja slúži na výpočet\nďalšej plánovanej výmeny podľa km\nalebo dátumu (mesiace od poslednej výmeny).",
            Dock = DockStyle.Fill,
            ForeColor = Color.FromArgb(80, 80, 80),
            Height = 52
        };
        layout.Controls.Add(lblInfo, 0, 1);
        layout.SetColumnSpan(lblInfo, 2);

        // Interval KM
        chkKM = new CheckBox
        {
            Text = "Interval podľa kilometrov:", Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleLeft
        };
        chkKM.CheckedChanged += (s, e) => numKM.Enabled = chkKM.Checked;
        layout.Controls.Add(chkKM, 0, 2);

        numKM = new NumericUpDown
        {
            Dock = DockStyle.Fill, Minimum = 1000, Maximum = 100000,
            Value = 15000, Increment = 1000, ThousandsSeparator = true, Enabled = false
        };
        layout.Controls.Add(numKM, 1, 2);

        // Príklady KM
        var lblKMPr = new Label
        {
            Text = "Typicky: 10 000, 15 000, 20 000 km",
            Dock = DockStyle.Fill, ForeColor = Color.Gray,
            Font = new Font("Segoe UI", 8f), Height = 18
        };
        layout.Controls.Add(lblKMPr, 0, 3);
        layout.SetColumnSpan(lblKMPr, 2);

        // Interval mesiace
        chkMesiace = new CheckBox
        {
            Text = "Interval podľa mesiacov:", Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleLeft
        };
        chkMesiace.CheckedChanged += (s, e) => numMesiace.Enabled = chkMesiace.Checked;
        layout.Controls.Add(chkMesiace, 0, 4);

        numMesiace = new NumericUpDown
        {
            Dock = DockStyle.Fill, Minimum = 1, Maximum = 60,
            Value = 12, Enabled = false
        };
        layout.Controls.Add(numMesiace, 1, 4);

        var lblMesPr = new Label
        {
            Text = "Typicky: 12 mesiacov (1 rok), 6, 24 mesiacov",
            Dock = DockStyle.Fill, ForeColor = Color.Gray,
            Font = new Font("Segoe UI", 8f), Height = 18
        };
        layout.Controls.Add(lblMesPr, 0, 5);
        layout.SetColumnSpan(lblMesPr, 2);

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

        Controls.Add(layout);
        Controls.Add(pnlBtn);
        AcceptButton = btnUlozit;
        CancelButton = btnZrusit;
    }

    private void NaplnFormular()
    {
        if (_nastavenia.IntervalKM.HasValue)
        {
            chkKM.Checked = true;
            numKM.Value = _nastavenia.IntervalKM.Value;
        }
        if (_nastavenia.IntervalMesiace.HasValue)
        {
            chkMesiace.Checked = true;
            numMesiace.Value = _nastavenia.IntervalMesiace.Value;
        }
    }

    private void BtnUlozit_Click(object? sender, EventArgs e)
    {
        _nastavenia.IntervalKM = chkKM.Checked ? (int)numKM.Value : null;
        _nastavenia.IntervalMesiace = chkMesiace.Checked ? (int)numMesiace.Value : null;
        DialogResult = DialogResult.OK;
        Close();
    }
}
