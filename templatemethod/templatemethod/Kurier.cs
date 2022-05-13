internal class Kurier : Inpost
{
    public Kurier(object zawartosc, Klient nadawca, Klient odbiorca) : base(zawartosc, nadawca, odbiorca) { }

    public override void DostawPaczke()
    {
        Console.WriteLine("Paczka już jest do odbioru.");
        dostawione = wyslane;
    }

    public override void NadajPaczke()
    {
        dostawione.Odbiorca.OtrzymajPaczke(dostawione.Paczka);
    }
}
