namespace ServisnaKniha.Models;

public class ServisnyTermin
{
    public int Id { get; set; }
    public int AutoId { get; set; }
    public string AutoNazov { get; set; } = string.Empty;
    public string Nazov { get; set; } = string.Empty;
    public string Popis { get; set; } = string.Empty;
    public DateTime? DatumTerminu { get; set; }
    public int? KMTerminu { get; set; }
    public int? OpakovanieDni { get; set; }
    public int? OpakovaniKM { get; set; }
    public bool JeAktivny { get; set; } = true;
    public DateTime? PosledneVykonanie { get; set; }
    public int? KMPoslednehoVykonania { get; set; }
    public string Poznamka { get; set; } = string.Empty;

    public StavTerminu GetStav(int aktualneKM)
    {
        bool datumPrekoroceny = DatumTerminu.HasValue && DatumTerminu.Value < DateTime.Today;
        bool kmPrekorocene = KMTerminu.HasValue && aktualneKM >= KMTerminu.Value;
        bool datumBlizko = DatumTerminu.HasValue && DatumTerminu.Value <= DateTime.Today.AddDays(30);
        bool kmBlizko = KMTerminu.HasValue && aktualneKM >= KMTerminu.Value - 500;

        if (datumPrekoroceny || kmPrekorocene) return StavTerminu.Prekoroceny;
        if (datumBlizko || kmBlizko) return StavTerminu.Blizko;
        return StavTerminu.VPoriadku;
    }
}

public enum StavTerminu
{
    VPoriadku,
    Blizko,
    Prekoroceny
}
