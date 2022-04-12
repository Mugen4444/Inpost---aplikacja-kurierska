﻿internal class Wydzial : Inpost
{
    // Tworzenie paczek
    public override Paczka PrzygotujPaczke(string rozmiar)
    {
        switch (rozmiar)
        {
            case "duza":
                return new PaczkaDuza();
            case "srednia":
                return new PaczkaSrednia();
            case "mala":
                return new PaczkaMala();
            default:
                throw new ArgumentException("Rozmiar " + rozmiar + " jest nieobslugiwany w Wydziale/Kurierem");
        }
    }
}
