namespace putovanjeApp1.Models;

public class User
{
    public Guid? Guid { get; set; }
    public string? Ime { get; set; }
    public string? Email { get; set; }
    public string? PasswordHash { get; set; }
    public List<string> Interesovanja { get; set; } = new();
}

