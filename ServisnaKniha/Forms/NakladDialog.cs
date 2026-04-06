using ServisnaKniha.Models;

namespace ServisnaKniha.Forms;

public partial class NakladDialog : Form
{
    private readonly Naklad _naklad;
    private readonly bool _jeNovy;
    private readonly List<Auto> _auta;

    public Naklad Vysledok => _naklad;

    public NakladDialog(List<Auto> auta, Naklad? naklad = null, int? predvyberAutoId = null)
    {
        _auta = auta;
        _jeNovy = naklad == null;
        _naklad = naklad ?? new Naklad { Datum = DateTime.Today, Mena = "EUR" };
        if (_jeNovy && predvyberAutoId.HasValue) _naklad.AutoId = predvyberAutoId.Value;
        InitializeComponent();
        Text = _jeNovy ? "Pridať náklad" : "Upraviť náklad";
        foreach (var a in _auta) cboAuto.Items.Add(a);
        NaplnFormular();
    }

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
