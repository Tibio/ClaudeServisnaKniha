namespace ServisnaKniha.Models;

public class Auto
{
    public int Id { get; set; }
    public string SPZ { get; set; } = string.Empty;
    public string Znacka { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int RokVyroby { get; set; }
    public string VIN { get; set; } = string.Empty;
    public string TypMotora { get; set; } = string.Empty;
    public double ObjemMotora { get; set; }
    public int VykonKW { get; set; }
    public string Farba { get; set; } = string.Empty;
    public int AktualneKM { get; set; }
    public DateTime DatumSTK { get; set; }
    public DateTime DatumEK { get; set; }
    public string Poznamka { get; set; } = string.Empty;
    public DateTime DatumPridania { get; set; } = DateTime.Now;

    public override string ToString() => $"{Znacka} {Model} ({SPZ})";
}
