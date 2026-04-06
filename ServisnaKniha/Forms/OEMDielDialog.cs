using ServisnaKniha.Models;

namespace ServisnaKniha.Forms;

public partial class OEMDielDialog : Form
{
    private readonly OEMDiel _diel;
    private readonly bool _jeNovy;
    private readonly List<Auto> _auta;

    public OEMDiel Vysledok => _diel;

    public OEMDielDialog(List<Auto> auta, OEMDiel? diel = null, int? predvyberAutoId = null)
    {
        _auta = auta;
        _jeNovy = diel == null;
        _diel = diel ?? new OEMDiel { DatumMontaze = DateTime.Today };
        if (_jeNovy && predvyberAutoId.HasValue) _diel.AutoId = predvyberAutoId.Value;
        InitializeComponent();
        Text = _jeNovy ? "Pridať OEM diel" : "Upraviť OEM diel";
        foreach (var a in _auta) cboAuto.Items.Add(a);
        NaplnFormular();
    }

    private void NumPocet_ValueChanged(object? sender, EventArgs e) => AktualizujCelkovu();
    private void NumCena_ValueChanged(object? sender, EventArgs e) => AktualizujCelkovu();

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
