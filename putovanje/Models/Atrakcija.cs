namespace putovanjeApp1.Models;

public class Atrakcija
{

    public Guid? guid { get; set; } = Guid.NewGuid();

    public string? ime { get; set; }
    public string?   tip { get; set; }   // muzej, planina, plaža itd.
}
