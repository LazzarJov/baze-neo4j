namespace putovanjeApp1.Dtos
{
    public class ReviewDto
    {
        public Guid? Guid { get; set; }
        public string? Tekst { get; set; }
        public int Ocena { get; set; }
        public Guid? UserGuid { get; set; }
        public Guid? AtrakcijaGuid { get; set; }
    }
}
