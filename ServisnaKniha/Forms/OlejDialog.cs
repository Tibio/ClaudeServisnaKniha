using ServisnaKniha.Models;

namespace ServisnaKniha.Forms;

public partial class OlejDialog : Form
{
    private readonly OlejZaznam _zaznam;
    private readonly bool _jeNovy;
    private readonly List<Auto> _auta;

    public OlejZaznam Vysledok => _zaznam;

    public OlejDialog(List<Auto> auta, OlejZaznam? zaznam = null, int? predvyberAutoId = null)
    {
        _auta = auta;
        _jeNovy = zaznam == null;
        _zaznam = zaznam ?? new OlejZaznam { DatumVymeny = DateTime.Today };
        if (_jeNovy && predvyberAutoId.HasValue) _zaznam.AutoId = predvyberAutoId.Value;
        InitializeComponent();
        Text = _jeNovy ? "Nová výmena oleja" : "Upraviť výmenu oleja";
        foreach (var a in _auta) cboAuto.Items.Add(a);
        NaplnFormular();
    }

    private void NumCenaOleja_ValueChanged(object? sender, EventArgs e) => AktualizujCelkovu();
    private void NumCenaVymeny_ValueChanged(object? sender, EventArgs e) => AktualizujCelkovu();

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
