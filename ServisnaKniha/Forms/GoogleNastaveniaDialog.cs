using ServisnaKniha.Models;

namespace ServisnaKniha.Forms;

public partial class GoogleNastaveniaDialog : Form
{
    public AppSettings Vysledok { get; }

    public GoogleNastaveniaDialog(AppSettings settings)
    {
        Vysledok = settings;
        InitializeComponent();
        NaplnFormular();
    }

    private void NaplnFormular()
    {
        txtClientId.Text = Vysledok.GoogleClientId;
        txtClientSecret.Text = Vysledok.GoogleClientSecret;
        txtFolderName.Text = Vysledok.GoogleDriveFolderName;
    }

    private void BtnUlozit_Click(object? sender, EventArgs e)
    {
        Vysledok.GoogleClientId = txtClientId.Text.Trim();
        Vysledok.GoogleClientSecret = txtClientSecret.Text.Trim();
        Vysledok.GoogleDriveFolderName = string.IsNullOrWhiteSpace(txtFolderName.Text)
            ? "ServisnaKniha_Zalohy" : txtFolderName.Text.Trim();
        Vysledok.Save();
        DialogResult = DialogResult.OK;
        Close();
    }
}
