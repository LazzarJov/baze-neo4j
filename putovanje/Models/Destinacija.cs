namespace putovanjeApp1.Models;

public class Destinacija
{


    public Guid? guid { get; set; } = Guid.NewGuid();


    public string? ime { get; set; }
    public string?  drzava { get; set; }
    public string? opis { get; set; }
    public List<string> tagovi { get; set; } = new();
}
