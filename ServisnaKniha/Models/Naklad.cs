namespace ServisnaKniha.Models;

public class Naklad
{
    public int Id { get; set; }
    public int AutoId { get; set; }
    public string AutoNazov { get; set; } = string.Empty;
    public int? ServisnyZaznamId { get; set; }
    public DateTime Datum { get; set; }
    public string Kategoria { get; set; } = string.Empty;
    public string Popis { get; set; } = string.Empty;
    public decimal Suma { get; set; }
    public string Mena { get; set; } = "EUR";
    public string Doklad { get; set; } = string.Empty;
    public string Poznamka { get; set; } = string.Empty;
}

public static class KategoriaNakladu
{
    public static readonly string[] Kategorie =
    [
        "Servis / Oprava",
        "Diely",
        "Pneumatiky",
        "Palivo",
        "Poistenie",
        "STK / EK",
        "Diaľničná známka",
        "Parkovanie",
        "Umývanie",
        "Iné"
    ];
}
