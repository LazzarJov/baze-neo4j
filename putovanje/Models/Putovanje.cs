namespace putovanjeApp1.Models;

public class Putovanje
{
    public Guid? guid { get; set; } = Guid.NewGuid();

    public string? naziv { get; set; }
    public DateTime? datumPocetka { get; set; }
    public DateTime? datumZavrsetka { get; set; }
}
