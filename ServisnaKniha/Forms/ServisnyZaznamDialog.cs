using ServisnaKniha.Models;

namespace ServisnaKniha.Forms;

public partial class ServisnyZaznamDialog : Form
{
    private readonly ServisnyZaznam _zaznam;
    private readonly bool _jeNovy;
    private readonly List<Auto> _auta;

    public ServisnyZaznam Vysledok => _zaznam;

    public ServisnyZaznamDialog(List<Auto> auta, ServisnyZaznam? zaznam = null, int? predvyberAutoId = null)
    {
        _auta = auta;
        _jeNovy = zaznam == null;
        _zaznam = zaznam ?? new ServisnyZaznam { DatumServisu = DateTime.Today };
        if (_jeNovy && predvyberAutoId.HasValue) _zaznam.AutoId = predvyberAutoId.Value;
        InitializeComponent();
        Text = _jeNovy ? "Nový servisný záznam" : "Upraviť servisný záznam";
        foreach (var a in _auta) cboAuto.Items.Add(a);
        NaplnFormular();
    }

    private void NumCenaPrace_ValueChanged(object? sender, EventArgs e) => AktualizujCelkovu();
    private void NumCenaDielov_ValueChanged(object? sender, EventArgs e) => AktualizujCelkovu();

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
