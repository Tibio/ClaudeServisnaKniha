using ServisnaKniha.Models;

namespace ServisnaKniha.Forms;

public partial class OlejNastaveniaDialog : Form
{
    private readonly OlejNastavenia _nastavenia;
    private readonly string _autoNazov;

    public OlejNastavenia Vysledok => _nastavenia;

    public OlejNastaveniaDialog(OlejNastavenia nastavenia, string autoNazov)
    {
        _nastavenia = nastavenia;
        _autoNazov = autoNazov;
        InitializeComponent();
        lblAutoNazov.Text = $"Vozidlo: {_autoNazov}";
        NaplnFormular();
    }

    private void ChkKM_CheckedChanged(object? sender, EventArgs e) => numKM.Enabled = chkKM.Checked;
    private void ChkMesiace_CheckedChanged(object? sender, EventArgs e) => numMesiace.Enabled = chkMesiace.Checked;

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
