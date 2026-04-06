using ServisnaKniha.Models;

namespace ServisnaKniha.Forms;

public partial class TerminDialog : Form
{
    private readonly ServisnyTermin _termin;
    private readonly bool _jeNovy;
    private readonly List<Auto> _auta;

    public ServisnyTermin Vysledok => _termin;

    public TerminDialog(List<Auto> auta, ServisnyTermin? termin = null, int? predvyberAutoId = null)
    {
        _auta = auta;
        _jeNovy = termin == null;
        _termin = termin ?? new ServisnyTermin();
        if (_jeNovy && predvyberAutoId.HasValue) _termin.AutoId = predvyberAutoId.Value;
        InitializeComponent();
        Text = _jeNovy ? "Nový servisný termín" : "Upraviť servisný termín";
        foreach (var a in _auta) cboAuto.Items.Add(a);
        NaplnFormular();
    }

    private void ChkDatum_CheckedChanged(object? sender, EventArgs e) => dtpDatum.Enabled = chkDatum.Checked;
    private void ChkKM_CheckedChanged(object? sender, EventArgs e) => numKM.Enabled = chkKM.Checked;
    private void ChkOpakDni_CheckedChanged(object? sender, EventArgs e) => numOpakDni.Enabled = chkOpakDni.Checked;
    private void ChkOpakKM_CheckedChanged(object? sender, EventArgs e) => numOpakKM.Enabled = chkOpakKM.Checked;

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
