namespace putovanjeApp1.Models;

public class User
{
    public Guid? guid { get; set; } = Guid.NewGuid();
    public string? ime { get; set; }
    public string? email { get; set; }
    public string? passwordHash { get; set; }
    public List<string> interesovanja { get; set; } = new();
}

