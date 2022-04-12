internal abstract class Inpost
{
    // Tworzenie paczek
    public abstract Paczka PrzygotujPaczke(string rozmiar);

    // Tworzenie jednostek
    public static Inpost Jednostka(string nazwa)
    {
        switch (nazwa)
        {
            case "wydzial":
                return new Wydzial();
            case "paczkomat":
                return new Paczkomat();
            case "kurier":
                return new Kurier();
            default:
                throw new ArgumentException("Nieznany rodzaj jednostki: " + nazwa);
        }
    }
}