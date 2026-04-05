namespace ServisnaKniha.Models;

public class OlejZaznam
{
    public int Id { get; set; }
    public int AutoId { get; set; }
    public string AutoNazov { get; set; } = string.Empty;
    public DateTime DatumVymeny { get; set; }
    public int StavKM { get; set; }
    public string ZnackaOleja { get; set; } = string.Empty;
    public string ViskozitaOleja { get; set; } = string.Empty;
    public double ObjemOleja { get; set; }
    public string FilterOleja { get; set; } = string.Empty;
    public decimal CenaOleja { get; set; }
    public decimal CenaVymeny { get; set; }
    public decimal CenaCelkom => CenaOleja + CenaVymeny;
    public string Servis { get; set; } = string.Empty;
    public string Poznamka { get; set; } = string.Empty;
    public DateTime DatumPridania { get; set; } = DateTime.Now;
}

public class OlejNastavenia
{
    public int Id { get; set; }
    public int AutoId { get; set; }
    public int? IntervalKM { get; set; }
    public int? IntervalMesiace { get; set; }
}

public static class OlejData
{
    public static readonly string[] Znacky =
    [
        "Agip / Eni", "Aral", "Bardahl", "Castrol", "Comma", "Elf",
        "Eurol", "Fuchs", "Gulf", "Havoline", "Helix (Shell)", "Kroon Oil",
        "Liqui Moly", "Lukoil", "Mannol", "Mobil", "Motul",
        "Pennzoil", "Petronas", "Putoline", "Quaker State",
        "Ravenol", "Repsol", "Shell", "Total", "Valvoline",
        "Wolf Oil", "ZIC", "Iné"
    ];

    public static readonly string[] Viskozity =
    [
        "0W-8", "0W-16", "0W-20", "0W-30", "0W-40",
        "5W-20", "5W-30", "5W-40", "5W-50",
        "10W-30", "10W-40", "10W-60",
        "15W-40", "15W-50",
        "20W-50"
    ];

    public static readonly string[] Filtre =
    [
        "Originálny OEM", "Mann-Filter", "Mahle", "Hengst",
        "Bosch", "Champion", "Febi", "Knecht", "Purflux",
        "UFI", "WIX", "Iné"
    ];
}
