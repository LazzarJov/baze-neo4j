namespace putovanjeApp1.Dtos
{
    public class UserDto
    {
        public string? ime { get; set; }
        public string? email { get; set; }
        public List<string> interesovanja { get; set; } = new();
        public List<Guid> putovanja { get; set; } = new(); // korisnik ima više putovanja
       
    }
}
 