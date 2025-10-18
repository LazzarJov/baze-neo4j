namespace putovanjeApp1.Dtos
{
    public class PutovanjeDTO
    {
        
        public string? Naziv { get; set; }
        public DateTime? DatumPocetka { get; set; }
        public DateTime? DatumZavrsetka { get; set; }

        public List<Guid> Atrakcije { get; set; } = new(); // lista destinacija koje obuhvata
        public List<Guid> Aktivnosti { get; set; } = new(); // aktivnosti u putovanju
    }
}
