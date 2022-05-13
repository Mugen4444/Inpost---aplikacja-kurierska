internal class Paczkomat : Inpost
{
    public Paczkomat(object zawartosc, Klient nadawca, Klient odbiorca) : base(zawartosc, nadawca, odbiorca) { }

    public override void DostawPaczke()
    {
        Console.WriteLine("Paczka już jest do odbioru.");
        dostawione = wyslane;
    }

    public override void NadajPaczke() { }

    public Paczka OdbierzPaczke(Klient odbiorca)
    {
        if (odbiorca.ImieNazwisko.Equals(dostawione.Odbiorca.ImieNazwisko))
        {
            return dostawione.Paczka;
        }
        else
        {
            throw new ApplicationException("Nie mozesz odebrac tej paczki.");
        }
    }
}
