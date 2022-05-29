namespace mvc.Inpost
{
    public class Paczka
    {
        public int? Id { get; set; }
        public string? Zawartosc { get; set; }
        public Klient? Nadawca { get; set; }
        public Klient? Odbiorca { get; set; }
        public DateTime DataDodania { get; } = DateTime.Now;
    }
}
