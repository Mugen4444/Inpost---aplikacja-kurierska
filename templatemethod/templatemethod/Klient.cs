internal class Klient
{
    public string Imie { get; } = new string[] { "Jan", "Anna", "Grzegorz" }[new Random().Next(3)];
    public string Nazwisko { get; } = new string[] { "Klepacz", "Waś", "Matusiewicz" }[new Random().Next(3)];
    public string ImieNazwisko { get => Imie + " " + Nazwisko; }

    public Inpost Wyslij(string sposob, Klient odbiorca)
    {
        var zawartosc = "laptop";

        Inpost inpost;

        switch (sposob)
        {
            case "kurier":
                inpost = new Kurier(zawartosc, this, odbiorca);
                break;
            case "paczkomat":
                inpost = new Paczkomat(zawartosc, this, odbiorca);
                break;
            default:
                inpost = new Paczkomat(zawartosc, this, odbiorca);
                break;
        }

        return inpost;
    }

    public void OtrzymajPaczke(Paczka paczka)
    {
        Console.WriteLine($"{Imie} otrzymał/ła paczkę: " + (string)paczka.Zawartosc);
    }

    public void OdbierzWPaczkomacie(Paczkomat paczkomat)
    {
        OtrzymajPaczke(paczkomat.OdbierzPaczke(this));
    }
}
