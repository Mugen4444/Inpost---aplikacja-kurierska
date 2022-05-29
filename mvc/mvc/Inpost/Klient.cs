namespace mvc.Inpost
{
    public class Klient
    {
        public int? Id { get; set; }
        public string? Imie { get; set; }
        public string? Nazwisko { get; set; }
        public string? NrTelefonu { get; set; }
        public string? AdresZamieszkania { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj is null) return false;

            var k = obj as Klient;

            if (k is null) return false;

            return (
                    k.Imie == Imie &&
                    k.Nazwisko == Nazwisko &&
                    k.NrTelefonu == NrTelefonu &&
                    k.AdresZamieszkania == k.AdresZamieszkania
                );
        }

        public override int GetHashCode()
        {
            if (String.IsNullOrEmpty(NrTelefonu))
                throw new Exception("NrTelefonu nie może być pusty");

            return NrTelefonu.GetHashCode();
        }

        public string ImieNazwisko => Imie + " " + Nazwisko;
    }
}
