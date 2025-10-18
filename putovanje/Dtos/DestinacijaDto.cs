namespace putovanjeApp1.Dtos
{
    public class DestinacijaDTO
    {
        public Guid? Guid { get; set; }
        public string? Ime { get; set; }
        public string? Drzava { get; set; }
        public string? Opis { get; set; }
        public List<string> Tagovi { get; set; } = new();
    }
}
