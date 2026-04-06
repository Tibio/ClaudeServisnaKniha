using ServisnaKniha.Database;
using ServisnaKniha.Models;
using System.Drawing;
using System.Windows.Forms;

namespace ServisnaKniha.Forms;

public partial class KMSledovaniePanel : UserControl
{
    private readonly DatabaseManager _db;
    private Auto? _aktualneAuto;

    private ComboBox cboAuto = null!;
    private Label lblAktualneKM = null!;
    private DataGridView dgvKM = null!;

    // Vstup nového KM záznamu
    private DateTimePicker dtpDatum = null!;
    private NumericUpDown numKM = null!;
    private TextBox txtPoznamka = null!;

    public KMSledovaniePanel(DatabaseManager db)
    {
        _db = db;
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        Dock = DockStyle.Fill;
        Font = new Font("Segoe UI", 9f);

        // Horná lišta — výber auta
        var pnlTop = new Panel { Dock = DockStyle.Top, Height = 45, BackColor = Color.FromArgb(240, 244, 250), Padding = new Padding(10, 8, 10, 8) };
        var lblVoz = new Label { Text = "Vozidlo:", AutoSize = true, Location = new Point(10, 12) };
        cboAuto = new ComboBox
        {
            DropDownStyle = ComboBoxStyle.DropDownList,
            Location = new Point(70, 9), Width = 280
        };
        cboAuto.SelectedIndexChanged += (s, e) => NacitajKM();

        lblAktualneKM = new Label
        {
            AutoSize = true, Location = new Point(365, 12),
            Font = new Font("Segoe UI", 10f, FontStyle.Bold),
            ForeColor = Color.FromArgb(0, 100, 180)
        };

        pnlTop.Controls.AddRange([lblVoz, cboAuto, lblAktualneKM]);

        // Panel na pridanie záznamu
        var pnlPridat = new Panel
        {
            Dock = DockStyle.Top, Height = 55,
            BackColor = Color.FromArgb(248, 252, 248),
            Padding = new Padding(10, 8, 10, 8)
        };
        pnlPridat.BorderStyle = BorderStyle.None;

        var lblDat = new Label { Text = "Dátum:", AutoSize = true, Location = new Point(10, 15) };
        dtpDatum = new DateTimePicker { Location = new Point(60, 11), Width = 110, Format = DateTimePickerFormat.Short };
        var lblKm = new Label { Text = "Km:", AutoSize = true, Location = new Point(180, 15) };
        numKM = new NumericUpDown
        {
            Location = new Point(205, 11), Width = 110, Maximum = 9999999,
            ThousandsSeparator = true, DecimalPlaces = 0
        };
        var lblPoz = new Label { Text = "Poznámka:", AutoSize = true, Location = new Point(325, 15) };
        txtPoznamka = new TextBox { Location = new Point(400, 11), Width = 150 };
        var btnPridat = new Button
        {
            Text = "Pridať záznam", Location = new Point(560, 10), Size = new Size(110, 28),
            BackColor = Color.FromArgb(40, 167, 69), ForeColor = Color.White, FlatStyle = FlatStyle.Flat
        };
        btnPridat.FlatAppearance.BorderSize = 0;
        btnPridat.Click += BtnPridat_Click;

        pnlPridat.Controls.AddRange([lblDat, dtpDatum, lblKm, numKM, lblPoz, txtPoznamka, btnPridat]);

        // Tabuľka
        dgvKM = VytvorGrid();

        // Panel tlačidiel pod gridom
        var pnlBtn = new Panel { Dock = DockStyle.Bottom, Height = 40, BackColor = Color.FromArgb(245, 245, 245) };
        var btnVymaz = new Button
        {
            Text = "Vymazať záznam", Location = new Point(10, 7), Size = new Size(130, 26),
            BackColor = Color.FromArgb(220, 53, 69), ForeColor = Color.White, FlatStyle = FlatStyle.Flat
        };
        btnVymaz.FlatAppearance.BorderSize = 0;
        btnVymaz.Click += BtnVymaz_Click;
        pnlBtn.Controls.Add(btnVymaz);

        Controls.Add(dgvKM);
        Controls.Add(pnlPridat);
        Controls.Add(pnlTop);
        Controls.Add(pnlBtn);
    }

    private static DataGridView VytvorGrid()
    {
        var dgv = new DataGridView
        {
            Dock = DockStyle.Fill,
            ReadOnly = true,
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            MultiSelect = false,
            RowHeadersVisible = false,
            BackgroundColor = Color.White,
            BorderStyle = BorderStyle.None,
            Font = new Font("Segoe UI", 9f),
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            GridColor = Color.FromArgb(220, 220, 220)
        };
        dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(52, 73, 94);
        dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
        dgv.ColumnHeadersHeight = 32;
        dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 248, 248);

        dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "Id", Visible = false });
        dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "Datum", HeaderText = "Dátum", FillWeight = 20 });
        dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "StavKM", HeaderText = "Stav km", FillWeight = 20, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight } });
        dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "Poznamka", HeaderText = "Poznámka", FillWeight = 60 });

        return dgv;
    }

    public void NacitajAuta()
    {
        var auta = _db.GetVsetkyAuta();
        cboAuto.Items.Clear();
        foreach (var a in auta) cboAuto.Items.Add(a);
        if (cboAuto.Items.Count > 0) cboAuto.SelectedIndex = 0;
    }

    private void NacitajKM()
    {
        _aktualneAuto = cboAuto.SelectedItem as Auto;
        dgvKM.Rows.Clear();

        if (_aktualneAuto == null)
        {
            lblAktualneKM.Text = "";
            return;
        }

        lblAktualneKM.Text = $"Aktuálne: {_aktualneAuto.AktualneKM:N0} km";
        numKM.Value = _aktualneAuto.AktualneKM;

        var zaznamy = _db.GetKMZaznamy(_aktualneAuto.Id);
        int? predchKM = null;
        foreach (var z in zaznamy)
        {
            int rozdiel = predchKM.HasValue ? z.StavKM - predchKM.Value : 0;
            string rozdielText = predchKM.HasValue ? $"+{rozdiel:N0} km" : "";
            dgvKM.Rows.Add(z.Id, z.Datum.ToString("dd.MM.yyyy"), $"{z.StavKM:N0} km", string.IsNullOrEmpty(z.Poznamka) ? rozdielText : $"{z.Poznamka} ({rozdielText})");
            predchKM = z.StavKM;
        }
    }

    private void BtnPridat_Click(object? sender, EventArgs e)
    {
        if (_aktualneAuto == null)
        { MessageBox.Show("Vyberte vozidlo.", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
        if (numKM.Value <= 0)
        { MessageBox.Show("Zadajte stav kilometrov.", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

        _db.PridajKMZaznam(new KMZaznam
        {
            AutoId = _aktualneAuto.Id,
            Datum = dtpDatum.Value,
            StavKM = (int)numKM.Value,
            Poznamka = txtPoznamka.Text.Trim()
        });

        txtPoznamka.Clear();

        // Obnov auto z DB
        var auto = _db.GetAuto(_aktualneAuto.Id);
        if (auto != null)
        {
            _aktualneAuto = auto;
            var idx = cboAuto.SelectedIndex;
            cboAuto.Items[idx] = auto;
            cboAuto.SelectedIndex = idx;
        }
        NacitajKM();
    }

    private void BtnVymaz_Click(object? sender, EventArgs e)
    {
        if (dgvKM.SelectedRows.Count == 0) return;
        int id = (int)dgvKM.SelectedRows[0].Cells["Id"].Value;
        if (MessageBox.Show("Naozaj vymazať tento km záznam?", "Potvrdenie",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        {
            _db.VymazKMZaznam(id);
            NacitajKM();
        }
    }
}
