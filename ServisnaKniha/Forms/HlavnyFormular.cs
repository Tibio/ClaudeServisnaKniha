using ServisnaKniha.Database;
using ServisnaKniha.Models;
using System.Drawing;
using System.Windows.Forms;

namespace ServisnaKniha.Forms;

public class HlavnyFormular : Form
{
    private readonly DatabaseManager _db;

    // Tabs
    private TabControl tabMain = null!;
    private TabPage tabAuta = null!;
    private TabPage tabServis = null!;
    private TabPage tabDiely = null!;
    private TabPage tabNaklady = null!;
    private TabPage tabTerminy = null!;
    private TabPage tabKM = null!;

    // Autá
    private DataGridView dgvAuta = null!;
    private ComboBox cboFilterServis = null!;
    private ComboBox cboFilterDiely = null!;
    private ComboBox cboFilterNaklady = null!;
    private ComboBox cboFilterTerminy = null!;

    // Gridy
    private DataGridView dgvServis = null!;
    private DataGridView dgvDiely = null!;
    private DataGridView dgvNaklady = null!;
    private DataGridView dgvTerminy = null!;

    // KM panel
    private KMSledovaniePanel pnlKM = null!;

    // Labels nákladov
    private Label lblNakladyCelkom = null!;

    public HlavnyFormular(DatabaseManager db)
    {
        _db = db;
        InitializeComponent();
        NacitajVsetko();
    }

    private void InitializeComponent()
    {
        Text = "Servisná kniha vozidiel";
        Size = new Size(1100, 720);
        MinimumSize = new Size(900, 600);
        StartPosition = FormStartPosition.CenterScreen;
        Font = new Font("Segoe UI", 9f);
        BackColor = Color.White;

        // Header
        var pnlHeader = new Panel
        {
            Dock = DockStyle.Top, Height = 56,
            BackColor = Color.FromArgb(30, 58, 95)
        };
        var lblTitle = new Label
        {
            Text = "  Servisná kniha vozidiel",
            Dock = DockStyle.Fill,
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 16f, FontStyle.Bold),
            TextAlign = ContentAlignment.MiddleLeft
        };
        var lblVersion = new Label
        {
            Text = "v1.0  ",
            Dock = DockStyle.Right,
            ForeColor = Color.FromArgb(180, 200, 230),
            Font = new Font("Segoe UI", 9f),
            TextAlign = ContentAlignment.MiddleRight,
            Width = 60
        };
        pnlHeader.Controls.AddRange([lblTitle, lblVersion]);

        // Tabs
        tabMain = new TabControl
        {
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI", 9.5f)
        };
        tabMain.DrawMode = TabDrawMode.OwnerDrawFixed;
        tabMain.ItemSize = new Size(130, 34);
        tabMain.DrawItem += TabMain_DrawItem;
        tabMain.SelectedIndexChanged += (s, e) => ObnovAktivnyTab();

        tabAuta = new TabPage("  Vozidlá");
        tabServis = new TabPage("  Servisné záznamy");
        tabDiely = new TabPage("  OEM diely");
        tabNaklady = new TabPage("  Náklady");
        tabTerminy = new TabPage("  Termíny servisu");
        tabKM = new TabPage("  Sledovanie km");

        tabMain.TabPages.AddRange([tabAuta, tabServis, tabDiely, tabNaklady, tabTerminy, tabKM]);

        BudujTabAuta();
        BudujTabServis();
        BudujTabDiely();
        BudujTabNaklady();
        BudujTabTerminy();
        BudujTabKM();

        Controls.Add(tabMain);
        Controls.Add(pnlHeader);
    }

    private static void TabMain_DrawItem(object? sender, DrawItemEventArgs e)
    {
        if (sender is not TabControl tc) return;
        var g = e.Graphics;
        var tab = tc.TabPages[e.Index];
        var rect = tc.GetTabRect(e.Index);

        bool selected = e.Index == tc.SelectedIndex;
        using var bgBrush = new SolidBrush(selected ? Color.FromArgb(30, 58, 95) : Color.FromArgb(245, 247, 250));
        g.FillRectangle(bgBrush, rect);

        using var txtBrush = new SolidBrush(selected ? Color.White : Color.FromArgb(60, 60, 60));
        using var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
        g.DrawString(tab.Text, new Font("Segoe UI", 9f, selected ? FontStyle.Bold : FontStyle.Regular), txtBrush, rect, sf);
    }

    // ==================== TAB AUTÁ ====================

    private void BudujTabAuta()
    {
        var pnl = new Panel { Dock = DockStyle.Fill };

        var toolbar = VytvorToolbar();
        var btnPridat = VytvorBtn("+ Pridať vozidlo", Color.FromArgb(40, 167, 69));
        var btnUpravit = VytvorBtn("✎ Upraviť", Color.FromArgb(0, 123, 255));
        var btnVymaz = VytvorBtn("✕ Vymazať", Color.FromArgb(220, 53, 69));
        var btnObnovit = VytvorBtn("↺ Obnoviť", Color.FromArgb(108, 117, 125));
        btnPridat.Click += BtnPridatAuto_Click;
        btnUpravit.Click += BtnUpravitAuto_Click;
        btnVymaz.Click += BtnVymazAuto_Click;
        btnObnovit.Click += (s, e) => NacitajAuta();
        toolbar.Controls.AddRange([btnPridat, btnUpravit, btnVymaz, btnObnovit]);

        dgvAuta = VytvorGrid();
        dgvAuta.Columns.Add(new DataGridViewTextBoxColumn { Name = "Id", Visible = false });
        dgvAuta.Columns.Add(new DataGridViewTextBoxColumn { Name = "SPZ", HeaderText = "ŠPZ", FillWeight = 12 });
        dgvAuta.Columns.Add(new DataGridViewTextBoxColumn { Name = "Znacka", HeaderText = "Značka", FillWeight = 14 });
        dgvAuta.Columns.Add(new DataGridViewTextBoxColumn { Name = "Model", HeaderText = "Model", FillWeight = 14 });
        dgvAuta.Columns.Add(new DataGridViewTextBoxColumn { Name = "Rok", HeaderText = "Rok", FillWeight = 8 });
        dgvAuta.Columns.Add(new DataGridViewTextBoxColumn { Name = "Motor", HeaderText = "Motor", FillWeight = 12 });
        dgvAuta.Columns.Add(new DataGridViewTextBoxColumn { Name = "KM", HeaderText = "Km", FillWeight = 10, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight } });
        dgvAuta.Columns.Add(new DataGridViewTextBoxColumn { Name = "STK", HeaderText = "STK do", FillWeight = 12 });
        dgvAuta.Columns.Add(new DataGridViewTextBoxColumn { Name = "EK", HeaderText = "EK do", FillWeight = 12 });
        dgvAuta.Columns.Add(new DataGridViewTextBoxColumn { Name = "VIN", HeaderText = "VIN", FillWeight = 16 });
        dgvAuta.CellDoubleClick += (s, e) => { if (e.RowIndex >= 0) BtnUpravitAuto_Click(s, e); };

        pnl.Controls.Add(dgvAuta);
        pnl.Controls.Add(toolbar);
        tabAuta.Controls.Add(pnl);
    }

    private void BtnPridatAuto_Click(object? sender, EventArgs e)
    {
        using var dlg = new AutoDialog();
        if (dlg.ShowDialog(this) == DialogResult.OK)
        {
            _db.PridajAuto(dlg.Vysledok);
            NacitajAuta();
        }
    }

    private void BtnUpravitAuto_Click(object? sender, EventArgs e)
    {
        if (dgvAuta.SelectedRows.Count == 0) return;
        int id = (int)dgvAuta.SelectedRows[0].Cells["Id"].Value;
        var auto = _db.GetAuto(id);
        if (auto == null) return;
        using var dlg = new AutoDialog(auto);
        if (dlg.ShowDialog(this) == DialogResult.OK)
        {
            _db.AktualizujAuto(dlg.Vysledok);
            NacitajAuta();
        }
    }

    private void BtnVymazAuto_Click(object? sender, EventArgs e)
    {
        if (dgvAuta.SelectedRows.Count == 0) return;
        var nazov = dgvAuta.SelectedRows[0].Cells["SPZ"].Value?.ToString();
        if (MessageBox.Show($"Naozaj vymazať vozidlo {nazov} aj so všetkými záznamami?",
            "Potvrdenie vymazania", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
        {
            int id = (int)dgvAuta.SelectedRows[0].Cells["Id"].Value;
            _db.VymazAuto(id);
            NacitajVsetko();
        }
    }

    private void NacitajAuta()
    {
        var auta = _db.GetVsetkyAuta();
        dgvAuta.Rows.Clear();
        foreach (var a in auta)
        {
            var row = dgvAuta.Rows.Add(a.Id, a.SPZ, a.Znacka, a.Model, a.RokVyroby,
                $"{a.ObjemMotora:N1}L {a.TypMotora}", $"{a.AktualneKM:N0}",
                a.DatumSTK.ToString("dd.MM.yyyy"),
                a.DatumEK.ToString("dd.MM.yyyy"), a.VIN);
            // Červené podfarbenie pri prekorocenej STK/EK
            var r = dgvAuta.Rows[row];
            if (a.DatumSTK < DateTime.Today || a.DatumEK < DateTime.Today)
                r.DefaultCellStyle.BackColor = Color.FromArgb(255, 230, 230);
            else if (a.DatumSTK < DateTime.Today.AddDays(30) || a.DatumEK < DateTime.Today.AddDays(30))
                r.DefaultCellStyle.BackColor = Color.FromArgb(255, 248, 220);
        }

        // Aktualizuj filtre
        AktualizujFilter(cboFilterServis, auta);
        AktualizujFilter(cboFilterDiely, auta);
        AktualizujFilter(cboFilterNaklady, auta);
        AktualizujFilter(cboFilterTerminy, auta);
        pnlKM.NacitajAuta();
    }

    private static void AktualizujFilter(ComboBox cbo, List<Auto> auta)
    {
        if (cbo == null) return;
        var selected = cbo.SelectedItem;
        cbo.Items.Clear();
        cbo.Items.Add("— Všetky vozidlá —");
        foreach (var a in auta) cbo.Items.Add(a);
        if (selected is Auto s && auta.Any(a => a.Id == s.Id))
            cbo.SelectedItem = auta.First(a => a.Id == s.Id);
        else
            cbo.SelectedIndex = 0;
    }

    // ==================== TAB SERVIS ====================

    private void BudujTabServis()
    {
        var pnl = new Panel { Dock = DockStyle.Fill };
        var toolbar = VytvorToolbar();

        var lblFilter = new Label { Text = "Vozidlo:", AutoSize = true, Margin = new Padding(5, 8, 2, 0) };
        cboFilterServis = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 230, Margin = new Padding(0, 5, 10, 0) };
        cboFilterServis.SelectedIndexChanged += (s, e) => NacitajServisneZaznamy();

        var btnPridat = VytvorBtn("+ Nový záznam", Color.FromArgb(40, 167, 69));
        var btnUpravit = VytvorBtn("✎ Upraviť", Color.FromArgb(0, 123, 255));
        var btnVymaz = VytvorBtn("✕ Vymazať", Color.FromArgb(220, 53, 69));
        btnPridat.Click += BtnPridatServis_Click;
        btnUpravit.Click += BtnUpravitServis_Click;
        btnVymaz.Click += BtnVymazServis_Click;
        toolbar.Controls.AddRange([lblFilter, cboFilterServis, btnPridat, btnUpravit, btnVymaz]);

        dgvServis = VytvorGrid();
        dgvServis.Columns.Add(new DataGridViewTextBoxColumn { Name = "Id", Visible = false });
        dgvServis.Columns.Add(new DataGridViewTextBoxColumn { Name = "Datum", HeaderText = "Dátum", FillWeight = 11 });
        dgvServis.Columns.Add(new DataGridViewTextBoxColumn { Name = "Auto", HeaderText = "Vozidlo", FillWeight = 18 });
        dgvServis.Columns.Add(new DataGridViewTextBoxColumn { Name = "KM", HeaderText = "Km", FillWeight = 10, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight } });
        dgvServis.Columns.Add(new DataGridViewTextBoxColumn { Name = "Typ", HeaderText = "Typ servisu", FillWeight = 15 });
        dgvServis.Columns.Add(new DataGridViewTextBoxColumn { Name = "Servis", HeaderText = "Servis / Dielňa", FillWeight = 16 });
        dgvServis.Columns.Add(new DataGridViewTextBoxColumn { Name = "Prace", HeaderText = "Práca (€)", FillWeight = 10, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight } });
        dgvServis.Columns.Add(new DataGridViewTextBoxColumn { Name = "Diely", HeaderText = "Diely (€)", FillWeight = 10, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight } });
        dgvServis.Columns.Add(new DataGridViewTextBoxColumn { Name = "Celkova", HeaderText = "Celkom (€)", FillWeight = 10, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Font = new Font("Segoe UI", 9f, FontStyle.Bold) } });
        dgvServis.CellDoubleClick += (s, e) => { if (e.RowIndex >= 0) BtnUpravitServis_Click(s, e); };

        pnl.Controls.Add(dgvServis);
        pnl.Controls.Add(toolbar);
        tabServis.Controls.Add(pnl);
    }

    private int? GetFilterAutoId(ComboBox cbo) =>
        cbo.SelectedItem is Auto a ? a.Id : null;

    private void BtnPridatServis_Click(object? sender, EventArgs e)
    {
        using var dlg = new ServisnyZaznamDialog(_db.GetVsetkyAuta(), predvyberAutoId: GetFilterAutoId(cboFilterServis));
        if (dlg.ShowDialog(this) == DialogResult.OK)
        {
            dlg.Vysledok.DatumPridania = DateTime.Now;
            _db.PridajServisnyZaznam(dlg.Vysledok);
            NacitajServisneZaznamy();
            NacitajAuta();
        }
    }

    private void BtnUpravitServis_Click(object? sender, EventArgs e)
    {
        if (dgvServis.SelectedRows.Count == 0) return;
        int id = (int)dgvServis.SelectedRows[0].Cells["Id"].Value;
        var zaznamy = _db.GetServisneZaznamy();
        var z = zaznamy.FirstOrDefault(x => x.Id == id);
        if (z == null) return;
        using var dlg = new ServisnyZaznamDialog(_db.GetVsetkyAuta(), z);
        if (dlg.ShowDialog(this) == DialogResult.OK)
        {
            _db.AktualizujServisnyZaznam(dlg.Vysledok);
            NacitajServisneZaznamy();
        }
    }

    private void BtnVymazServis_Click(object? sender, EventArgs e)
    {
        if (dgvServis.SelectedRows.Count == 0) return;
        if (MessageBox.Show("Naozaj vymazať tento servisný záznam?", "Potvrdenie",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        {
            _db.VymazServisnyZaznam((int)dgvServis.SelectedRows[0].Cells["Id"].Value);
            NacitajServisneZaznamy();
        }
    }

    private void NacitajServisneZaznamy()
    {
        var zaznamy = _db.GetServisneZaznamy(GetFilterAutoId(cboFilterServis));
        dgvServis.Rows.Clear();
        foreach (var z in zaznamy)
            dgvServis.Rows.Add(z.Id, z.DatumServisu.ToString("dd.MM.yyyy"), z.AutoNazov,
                $"{z.StavKM:N0}", z.TypServisu, z.Servis,
                $"{z.CenaPrace:N2}", $"{z.CenaDielov:N2}", $"{z.CelkovaCena:N2}");
    }

    // ==================== TAB DIELY ====================

    private void BudujTabDiely()
    {
        var pnl = new Panel { Dock = DockStyle.Fill };
        var toolbar = VytvorToolbar();

        var lblFilter = new Label { Text = "Vozidlo:", AutoSize = true, Margin = new Padding(5, 8, 2, 0) };
        cboFilterDiely = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 230, Margin = new Padding(0, 5, 10, 0) };
        cboFilterDiely.SelectedIndexChanged += (s, e) => NacitajDiely();

        var btnPridat = VytvorBtn("+ Pridať diel", Color.FromArgb(40, 167, 69));
        var btnUpravit = VytvorBtn("✎ Upraviť", Color.FromArgb(0, 123, 255));
        var btnVymaz = VytvorBtn("✕ Vymazať", Color.FromArgb(220, 53, 69));
        btnPridat.Click += BtnPridatDiel_Click;
        btnUpravit.Click += BtnUpravitDiel_Click;
        btnVymaz.Click += BtnVymazDiel_Click;
        toolbar.Controls.AddRange([lblFilter, cboFilterDiely, btnPridat, btnUpravit, btnVymaz]);

        dgvDiely = VytvorGrid();
        dgvDiely.Columns.Add(new DataGridViewTextBoxColumn { Name = "Id", Visible = false });
        dgvDiely.Columns.Add(new DataGridViewTextBoxColumn { Name = "Auto", HeaderText = "Vozidlo", FillWeight = 16 });
        dgvDiely.Columns.Add(new DataGridViewTextBoxColumn { Name = "Datum", HeaderText = "Dátum", FillWeight = 10 });
        dgvDiely.Columns.Add(new DataGridViewTextBoxColumn { Name = "Nazov", HeaderText = "Názov dielu", FillWeight = 20 });
        dgvDiely.Columns.Add(new DataGridViewTextBoxColumn { Name = "OEM", HeaderText = "OEM číslo", FillWeight = 14 });
        dgvDiely.Columns.Add(new DataGridViewTextBoxColumn { Name = "Vyrobca", HeaderText = "Výrobca", FillWeight = 12 });
        dgvDiely.Columns.Add(new DataGridViewTextBoxColumn { Name = "Kategoria", HeaderText = "Kategória", FillWeight = 12 });
        dgvDiely.Columns.Add(new DataGridViewTextBoxColumn { Name = "Pocet", HeaderText = "Ks", FillWeight = 5, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter } });
        dgvDiely.Columns.Add(new DataGridViewTextBoxColumn { Name = "Cena", HeaderText = "Cena (€)", FillWeight = 11, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight } });
        dgvDiely.Columns.Add(new DataGridViewTextBoxColumn { Name = "KM", HeaderText = "Km montáže", FillWeight = 10, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight } });
        dgvDiely.CellDoubleClick += (s, e) => { if (e.RowIndex >= 0) BtnUpravitDiel_Click(s, e); };

        pnl.Controls.Add(dgvDiely);
        pnl.Controls.Add(toolbar);
        tabDiely.Controls.Add(pnl);
    }

    private void BtnPridatDiel_Click(object? sender, EventArgs e)
    {
        using var dlg = new OEMDielDialog(_db.GetVsetkyAuta(), predvyberAutoId: GetFilterAutoId(cboFilterDiely));
        if (dlg.ShowDialog(this) == DialogResult.OK) { _db.PridajOEMDiel(dlg.Vysledok); NacitajDiely(); }
    }

    private void BtnUpravitDiel_Click(object? sender, EventArgs e)
    {
        if (dgvDiely.SelectedRows.Count == 0) return;
        int id = (int)dgvDiely.SelectedRows[0].Cells["Id"].Value;
        var diely = _db.GetOEMDiely();
        var d = diely.FirstOrDefault(x => x.Id == id);
        if (d == null) return;
        using var dlg = new OEMDielDialog(_db.GetVsetkyAuta(), d);
        if (dlg.ShowDialog(this) == DialogResult.OK) { _db.AktualizujOEMDiel(dlg.Vysledok); NacitajDiely(); }
    }

    private void BtnVymazDiel_Click(object? sender, EventArgs e)
    {
        if (dgvDiely.SelectedRows.Count == 0) return;
        if (MessageBox.Show("Naozaj vymazať tento diel?", "Potvrdenie",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        {
            _db.VymazOEMDiel((int)dgvDiely.SelectedRows[0].Cells["Id"].Value);
            NacitajDiely();
        }
    }

    private void NacitajDiely()
    {
        var diely = _db.GetOEMDiely(GetFilterAutoId(cboFilterDiely));
        dgvDiely.Rows.Clear();
        foreach (var d in diely)
            dgvDiely.Rows.Add(d.Id, d.AutoNazov, d.DatumMontaze.ToString("dd.MM.yyyy"),
                d.NazovDielu, d.OEMCislo, d.Vyrobca, d.Kategoria,
                d.Pocet, $"{d.CenaCelkom:N2}", $"{d.KMPriMontazi:N0}");
    }

    // ==================== TAB NÁKLADY ====================

    private void BudujTabNaklady()
    {
        var pnl = new Panel { Dock = DockStyle.Fill };
        var toolbar = VytvorToolbar();

        var lblFilter = new Label { Text = "Vozidlo:", AutoSize = true, Margin = new Padding(5, 8, 2, 0) };
        cboFilterNaklady = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 230, Margin = new Padding(0, 5, 10, 0) };
        cboFilterNaklady.SelectedIndexChanged += (s, e) => NacitajNaklady();

        var btnPridat = VytvorBtn("+ Pridať náklad", Color.FromArgb(40, 167, 69));
        var btnUpravit = VytvorBtn("✎ Upraviť", Color.FromArgb(0, 123, 255));
        var btnVymaz = VytvorBtn("✕ Vymazať", Color.FromArgb(220, 53, 69));
        btnPridat.Click += BtnPridatNaklad_Click;
        btnUpravit.Click += BtnUpravitNaklad_Click;
        btnVymaz.Click += BtnVymazNaklad_Click;
        toolbar.Controls.AddRange([lblFilter, cboFilterNaklady, btnPridat, btnUpravit, btnVymaz]);

        dgvNaklady = VytvorGrid();
        dgvNaklady.Columns.Add(new DataGridViewTextBoxColumn { Name = "Id", Visible = false });
        dgvNaklady.Columns.Add(new DataGridViewTextBoxColumn { Name = "Datum", HeaderText = "Dátum", FillWeight = 10 });
        dgvNaklady.Columns.Add(new DataGridViewTextBoxColumn { Name = "Auto", HeaderText = "Vozidlo", FillWeight = 20 });
        dgvNaklady.Columns.Add(new DataGridViewTextBoxColumn { Name = "Kategoria", HeaderText = "Kategória", FillWeight = 15 });
        dgvNaklady.Columns.Add(new DataGridViewTextBoxColumn { Name = "Popis", HeaderText = "Popis", FillWeight = 30 });
        dgvNaklady.Columns.Add(new DataGridViewTextBoxColumn { Name = "Suma", HeaderText = "Suma (€)", FillWeight = 12, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight, Font = new Font("Segoe UI", 9f, FontStyle.Bold) } });
        dgvNaklady.Columns.Add(new DataGridViewTextBoxColumn { Name = "Doklad", HeaderText = "Doklad č.", FillWeight = 13 });
        dgvNaklady.CellDoubleClick += (s, e) => { if (e.RowIndex >= 0) BtnUpravitNaklad_Click(s, e); };

        // Súčet
        var pnlSucet = new Panel { Dock = DockStyle.Bottom, Height = 32, BackColor = Color.FromArgb(240, 244, 250) };
        lblNakladyCelkom = new Label
        {
            Dock = DockStyle.Right, Width = 300, TextAlign = ContentAlignment.MiddleRight,
            Font = new Font("Segoe UI", 10f, FontStyle.Bold),
            ForeColor = Color.FromArgb(180, 30, 30), Padding = new Padding(0, 0, 15, 0)
        };
        pnlSucet.Controls.Add(lblNakladyCelkom);

        pnl.Controls.Add(dgvNaklady);
        pnl.Controls.Add(pnlSucet);
        pnl.Controls.Add(toolbar);
        tabNaklady.Controls.Add(pnl);
    }

    private void BtnPridatNaklad_Click(object? sender, EventArgs e)
    {
        using var dlg = new NakladDialog(_db.GetVsetkyAuta(), predvyberAutoId: GetFilterAutoId(cboFilterNaklady));
        if (dlg.ShowDialog(this) == DialogResult.OK) { _db.PridajNaklad(dlg.Vysledok); NacitajNaklady(); }
    }

    private void BtnUpravitNaklad_Click(object? sender, EventArgs e)
    {
        if (dgvNaklady.SelectedRows.Count == 0) return;
        int id = (int)dgvNaklady.SelectedRows[0].Cells["Id"].Value;
        var naklady = _db.GetNaklady();
        var n = naklady.FirstOrDefault(x => x.Id == id);
        if (n == null) return;
        using var dlg = new NakladDialog(_db.GetVsetkyAuta(), n);
        if (dlg.ShowDialog(this) == DialogResult.OK) { _db.AktualizujNaklad(dlg.Vysledok); NacitajNaklady(); }
    }

    private void BtnVymazNaklad_Click(object? sender, EventArgs e)
    {
        if (dgvNaklady.SelectedRows.Count == 0) return;
        if (MessageBox.Show("Naozaj vymazať tento náklad?", "Potvrdenie",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        {
            _db.VymazNaklad((int)dgvNaklady.SelectedRows[0].Cells["Id"].Value);
            NacitajNaklady();
        }
    }

    private void NacitajNaklady()
    {
        var filterAutoId = GetFilterAutoId(cboFilterNaklady);
        var naklady = _db.GetNaklady(filterAutoId);
        dgvNaklady.Rows.Clear();
        decimal sucet = 0;
        foreach (var n in naklady)
        {
            dgvNaklady.Rows.Add(n.Id, n.Datum.ToString("dd.MM.yyyy"), n.AutoNazov,
                n.Kategoria, n.Popis, $"{n.Suma:N2}", n.Doklad);
            sucet += n.Suma;
        }
        lblNakladyCelkom.Text = $"Celkové náklady: {sucet:N2} €   ";
    }

    // ==================== TAB TERMÍNY ====================

    private void BudujTabTerminy()
    {
        var pnl = new Panel { Dock = DockStyle.Fill };
        var toolbar = VytvorToolbar();

        var lblFilter = new Label { Text = "Vozidlo:", AutoSize = true, Margin = new Padding(5, 8, 2, 0) };
        cboFilterTerminy = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 230, Margin = new Padding(0, 5, 10, 0) };
        cboFilterTerminy.SelectedIndexChanged += (s, e) => NacitajTerminy();

        var btnPridat = VytvorBtn("+ Nový termín", Color.FromArgb(40, 167, 69));
        var btnUpravit = VytvorBtn("✎ Upraviť", Color.FromArgb(0, 123, 255));
        var btnVykonany = VytvorBtn("✔ Vykonané", Color.FromArgb(23, 162, 184));
        var btnVymaz = VytvorBtn("✕ Vymazať", Color.FromArgb(220, 53, 69));
        btnPridat.Click += BtnPridatTermin_Click;
        btnUpravit.Click += BtnUpravitTermin_Click;
        btnVykonany.Click += BtnVykonanyTermin_Click;
        btnVymaz.Click += BtnVymazTermin_Click;
        toolbar.Controls.AddRange([lblFilter, cboFilterTerminy, btnPridat, btnUpravit, btnVykonany, btnVymaz]);

        dgvTerminy = VytvorGrid();
        dgvTerminy.Columns.Add(new DataGridViewTextBoxColumn { Name = "Id", Visible = false });
        dgvTerminy.Columns.Add(new DataGridViewTextBoxColumn { Name = "Stav", HeaderText = "Stav", FillWeight = 8 });
        dgvTerminy.Columns.Add(new DataGridViewTextBoxColumn { Name = "Auto", HeaderText = "Vozidlo", FillWeight = 18 });
        dgvTerminy.Columns.Add(new DataGridViewTextBoxColumn { Name = "Nazov", HeaderText = "Názov", FillWeight = 20 });
        dgvTerminy.Columns.Add(new DataGridViewTextBoxColumn { Name = "DatumTerminu", HeaderText = "Termín dátum", FillWeight = 13 });
        dgvTerminy.Columns.Add(new DataGridViewTextBoxColumn { Name = "KMTerminu", HeaderText = "Termín km", FillWeight = 12, DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleRight } });
        dgvTerminy.Columns.Add(new DataGridViewTextBoxColumn { Name = "Opakovanie", HeaderText = "Opakovanie", FillWeight = 14 });
        dgvTerminy.Columns.Add(new DataGridViewTextBoxColumn { Name = "PosledneVyk", HeaderText = "Posl. vykonanie", FillWeight = 15 });
        dgvTerminy.CellDoubleClick += (s, e) => { if (e.RowIndex >= 0) BtnUpravitTermin_Click(s, e); };

        pnl.Controls.Add(dgvTerminy);
        pnl.Controls.Add(toolbar);
        tabTerminy.Controls.Add(pnl);
    }

    private void BtnPridatTermin_Click(object? sender, EventArgs e)
    {
        using var dlg = new TerminDialog(_db.GetVsetkyAuta(), predvyberAutoId: GetFilterAutoId(cboFilterTerminy));
        if (dlg.ShowDialog(this) == DialogResult.OK) { _db.PridajTermin(dlg.Vysledok); NacitajTerminy(); }
    }

    private void BtnUpravitTermin_Click(object? sender, EventArgs e)
    {
        if (dgvTerminy.SelectedRows.Count == 0) return;
        int id = (int)dgvTerminy.SelectedRows[0].Cells["Id"].Value;
        var terminy = _db.GetServisneTerminy();
        var t = terminy.FirstOrDefault(x => x.Id == id);
        if (t == null) return;
        using var dlg = new TerminDialog(_db.GetVsetkyAuta(), t);
        if (dlg.ShowDialog(this) == DialogResult.OK) { _db.AktualizujTermin(dlg.Vysledok); NacitajTerminy(); }
    }

    private void BtnVykonanyTermin_Click(object? sender, EventArgs e)
    {
        if (dgvTerminy.SelectedRows.Count == 0) return;
        int id = (int)dgvTerminy.SelectedRows[0].Cells["Id"].Value;
        var terminy = _db.GetServisneTerminy();
        var t = terminy.FirstOrDefault(x => x.Id == id);
        if (t == null) return;
        var auto = _db.GetAuto(t.AutoId);
        _db.OznacTerminVykonany(id, DateTime.Today, auto?.AktualneKM ?? 0);
        NacitajTerminy();
    }

    private void BtnVymazTermin_Click(object? sender, EventArgs e)
    {
        if (dgvTerminy.SelectedRows.Count == 0) return;
        if (MessageBox.Show("Naozaj vymazať tento termín?", "Potvrdenie",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        {
            _db.VymazTermin((int)dgvTerminy.SelectedRows[0].Cells["Id"].Value);
            NacitajTerminy();
        }
    }

    private void NacitajTerminy()
    {
        var filterAutoId = GetFilterAutoId(cboFilterTerminy);
        var auta = _db.GetVsetkyAuta();
        var terminy = _db.GetServisneTerminy(filterAutoId);
        dgvTerminy.Rows.Clear();

        foreach (var t in terminy)
        {
            var auto = auta.FirstOrDefault(a => a.Id == t.AutoId);
            var stav = t.GetStav(auto?.AktualneKM ?? 0);
            string stavText = stav switch
            {
                StavTerminu.Prekoroceny => "⚠ EXPIR.",
                StavTerminu.Blizko => "! Blíži",
                _ => "✓ OK"
            };
            string opakovanie = "";
            if (t.OpakovanieDni.HasValue) opakovanie += $"{t.OpakovanieDni}d ";
            if (t.OpakovaniKM.HasValue) opakovanie += $"{t.OpakovaniKM:N0}km";

            int rowIdx = dgvTerminy.Rows.Add(t.Id, stavText, t.AutoNazov, t.Nazov,
                t.DatumTerminu?.ToString("dd.MM.yyyy") ?? "-",
                t.KMTerminu.HasValue ? $"{t.KMTerminu:N0}" : "-",
                opakovanie.Trim(),
                t.PosledneVykonanie?.ToString("dd.MM.yyyy") ?? "-");

            var row = dgvTerminy.Rows[rowIdx];
            row.DefaultCellStyle.BackColor = stav switch
            {
                StavTerminu.Prekoroceny => Color.FromArgb(255, 220, 220),
                StavTerminu.Blizko => Color.FromArgb(255, 248, 200),
                _ => Color.White
            };
            if (!t.JeAktivny)
                row.DefaultCellStyle.ForeColor = Color.Gray;
        }
    }

    // ==================== TAB KM ====================

    private void BudujTabKM()
    {
        pnlKM = new KMSledovaniePanel(_db);
        tabKM.Controls.Add(pnlKM);
    }

    // ==================== POMOCNÉ METÓDY ====================

    private void ObnovAktivnyTab()
    {
        switch (tabMain.SelectedTab?.Name ?? "")
        {
            case var _ when tabMain.SelectedTab == tabServis: NacitajServisneZaznamy(); break;
            case var _ when tabMain.SelectedTab == tabDiely: NacitajDiely(); break;
            case var _ when tabMain.SelectedTab == tabNaklady: NacitajNaklady(); break;
            case var _ when tabMain.SelectedTab == tabTerminy: NacitajTerminy(); break;
        }
    }

    private void NacitajVsetko()
    {
        NacitajAuta();
        NacitajServisneZaznamy();
        NacitajDiely();
        NacitajNaklady();
        NacitajTerminy();
    }

    private static FlowLayoutPanel VytvorToolbar() => new()
    {
        Dock = DockStyle.Top,
        Height = 44,
        BackColor = Color.FromArgb(248, 249, 250),
        Padding = new Padding(8, 7, 8, 0),
        FlowDirection = FlowDirection.LeftToRight
    };

    private static Button VytvorBtn(string text, Color bgColor)
    {
        var btn = new Button
        {
            Text = text,
            AutoSize = true,
            MinimumSize = new Size(100, 28),
            Padding = new Padding(8, 0, 8, 0),
            BackColor = bgColor,
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Margin = new Padding(0, 0, 6, 0),
            Cursor = Cursors.Hand
        };
        btn.FlatAppearance.BorderSize = 0;
        return btn;
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
            GridColor = Color.FromArgb(220, 225, 230),
            RowTemplate = { Height = 26 },
            ColumnHeadersHeight = 32
        };
        dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(52, 73, 94);
        dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
        dgv.ColumnHeadersDefaultCellStyle.Padding = new Padding(4, 0, 0, 0);
        dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(247, 249, 252);
        dgv.EnableHeadersVisualStyles = false;
        return dgv;
    }
}
