namespace ServisnaKniha.Models;

public class OEMDiel
{
    public int Id { get; set; }
    public int AutoId { get; set; }
    public string AutoNazov { get; set; } = string.Empty;
    public int? ServisnyZaznamId { get; set; }
    public string NazovDielu { get; set; } = string.Empty;
    public string OEMCislo { get; set; } = string.Empty;
    public string Vyrobca { get; set; } = string.Empty;
    public string Kategoria { get; set; } = string.Empty;
    public int Pocet { get; set; } = 1;
    public decimal CenaKus { get; set; }
    public decimal CenaCelkom => Pocet * CenaKus;
    public DateTime DatumMontaze { get; set; }
    public int KMPriMontazi { get; set; }
    public string Poznamka { get; set; } = string.Empty;
}

public static class KategoriaDielu
{
    public static readonly string[] Kategorie =
    [
        "Motor",
        "Prevodovka",
        "Podvozok",
        "Brzdy",
        "Riadenie",
        "Elektrika",
        "Karoséria",
        "Interiér",
        "Klimatizácia",
        "Výfuk",
        "Palivový systém",
        "Chladiaci systém",
        "Filtre",
        "Pneumatiky / Kolesá",
        "Iné"
    ];
}
