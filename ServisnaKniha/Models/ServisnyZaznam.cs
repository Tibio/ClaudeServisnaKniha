namespace ServisnaKniha.Models;

public class ServisnyZaznam
{
    public int Id { get; set; }
    public int AutoId { get; set; }
    public string AutoNazov { get; set; } = string.Empty;
    public DateTime DatumServisu { get; set; }
    public int StavKM { get; set; }
    public string TypServisu { get; set; } = string.Empty;
    public string Popis { get; set; } = string.Empty;
    public string Servis { get; set; } = string.Empty;
    public decimal CelkovaCena { get; set; }
    public decimal CenaPrace { get; set; }
    public decimal CenaDielov { get; set; }
    public string Poznamka { get; set; } = string.Empty;
    public DateTime DatumPridania { get; set; } = DateTime.Now;
}

public static class TypServisu
{
    public static readonly string[] Typy =
    [
        "Výmena oleja",
        "Veľký servis",
        "Malý servis",
        "Oprava",
        "Výmena bŕzd",
        "Výmena pneumatík",
        "Technická kontrola",
        "Emisná kontrola",
        "Výmena rozvodov",
        "Klimatizácia",
        "Elektroinštalácia",
        "Karosárske práce",
        "Iné"
    ];
}
