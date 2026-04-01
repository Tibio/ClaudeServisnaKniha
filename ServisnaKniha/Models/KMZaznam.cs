namespace ServisnaKniha.Models;

public class KMZaznam
{
    public int Id { get; set; }
    public int AutoId { get; set; }
    public string AutoNazov { get; set; } = string.Empty;
    public DateTime Datum { get; set; }
    public int StavKM { get; set; }
    public string Poznamka { get; set; } = string.Empty;
}
