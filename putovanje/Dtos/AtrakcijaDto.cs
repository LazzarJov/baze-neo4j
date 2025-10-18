namespace putovanjeApp1.Dtos
{
    public class AtrakcijaDTO
    {
        public Guid? Guid { get; set; }
        public string? Ime { get; set; }
        public string? Tip { get; set; }

        public List<Guid> Komentari { get; set; } = new(); //komentari za atrakciju
    }
}
