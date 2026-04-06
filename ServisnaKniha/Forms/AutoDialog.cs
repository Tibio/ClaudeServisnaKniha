using ServisnaKniha.Models;

namespace ServisnaKniha.Forms;

public partial class AutoDialog : Form
{
    private readonly Auto _auto;
    private readonly bool _jeNovy;

    public Auto Vysledok => _auto;

    public AutoDialog(Auto? auto = null)
    {
        _jeNovy = auto == null;
        _auto = auto ?? new Auto { DatumSTK = DateTime.Today.AddYear(1), DatumEK = DateTime.Today.AddYear(2) };
        InitializeComponent();
        Text = _jeNovy ? "Pridať vozidlo" : "Upraviť vozidlo";
        NaplnFormular();
    }

    private void NaplnFormular()
    {
        txtSPZ.Text = _auto.SPZ;
        cboZnacka.Text = _auto.Znacka;
        txtModel.Text = _auto.Model;
        numRok.Value = _auto.RokVyroby == 0 ? DateTime.Now.Year : _auto.RokVyroby;
        txtVIN.Text = _auto.VIN;
        cboTypMotora.SelectedItem = string.IsNullOrEmpty(_auto.TypMotora) ? "Benzín" : _auto.TypMotora;
        if (cboTypMotora.SelectedIndex < 0) cboTypMotora.SelectedIndex = 0;
        numObjem.Value = (decimal)Math.Min(_auto.ObjemMotora, 10);
        numVykon.Value = _auto.VykonKW;
        txtFarba.Text = _auto.Farba;
        numKM.Value = _auto.AktualneKM;
        dtpSTK.Value = _auto.DatumSTK == DateTime.MinValue ? DateTime.Today.AddYears(1) : _auto.DatumSTK;
        dtpEK.Value = _auto.DatumEK == DateTime.MinValue ? DateTime.Today.AddYears(2) : _auto.DatumEK;
        txtPoznamka.Text = _auto.Poznamka;
    }

    private void BtnUlozit_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtSPZ.Text))
        { MessageBox.Show("Zadajte ŠPZ vozidla.", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
        if (string.IsNullOrWhiteSpace(cboZnacka.Text))
        { MessageBox.Show("Zadajte značku vozidla.", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
        if (string.IsNullOrWhiteSpace(txtModel.Text))
        { MessageBox.Show("Zadajte model vozidla.", "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

        _auto.SPZ = txtSPZ.Text.Trim().ToUpper();
        _auto.Znacka = cboZnacka.Text.Trim();
        _auto.Model = txtModel.Text.Trim();
        _auto.RokVyroby = (int)numRok.Value;
        _auto.VIN = txtVIN.Text.Trim().ToUpper();
        _auto.TypMotora = cboTypMotora.SelectedItem?.ToString() ?? "";
        _auto.ObjemMotora = (double)numObjem.Value;
        _auto.VykonKW = (int)numVykon.Value;
        _auto.Farba = txtFarba.Text.Trim();
        _auto.AktualneKM = (int)numKM.Value;
        _auto.DatumSTK = dtpSTK.Value;
        _auto.DatumEK = dtpEK.Value;
        _auto.Poznamka = txtPoznamka.Text.Trim();

        DialogResult = DialogResult.OK;
        Close();
    }
}

internal static class DateTimeExt
{
    public static DateTime AddYear(this DateTime dt, int years) => dt.AddYears(years);
}
