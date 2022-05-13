internal class Klient
{
    public string Imie { get; } = new string[] { "Jan", "Anna", "Grzegorz" }[new Random().Next(3)];
    public string Nazwisko { get; } = new string[] { "Klepacz", "Waś", "Matusiewicz" }[new Random().Next(3)];
    public string ImieNazwisko { get => Imie + " " + Nazwisko; }
}
