internal class Paczkomat : Inpost
{
    // Tworzenie paczek
    public override Paczka PrzygotujPaczke(string rozmiar)
    {
        switch (rozmiar)
        {
            case "srednia":
                return new PaczkaSrednia();
            case "mala":
                return new PaczkaMala();
            default:
                throw new ArgumentException("Rozmiar " + rozmiar + " jest nieobslugiwany w Paczkomacie");
        }
    }
}
