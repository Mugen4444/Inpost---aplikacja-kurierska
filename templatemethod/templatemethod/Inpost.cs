internal abstract class Inpost
{
    private Zamowienie zamowienie;
    protected Zamowienie? dostawione;
    protected Zamowienie? wyslane;

    public Inpost(object zawartosc, Klient nadawca, Klient odbiorca)
    {
        OtrzymajZamowienie(
            WygenerujZamowienie(
                    StworzPaczke(zawartosc),
                    nadawca,
                    odbiorca
                )
            );

        WyslijPaczke();
        DostawPaczke();
        NadajPaczke();
    }

    public Paczka StworzPaczke(object zawartosc)
    {
        return new Paczka(zawartosc);
    }

    public Zamowienie WygenerujZamowienie(Paczka paczka, Klient nadawca, Klient odbiorca)
    {
        return new Zamowienie(paczka, nadawca, odbiorca);
    }

    public void OtrzymajZamowienie(Zamowienie zamowienie)
    {
        this.zamowienie = zamowienie;
    }
    public void WyslijPaczke()
    {
        wyslane = zamowienie;
    }

    public abstract void DostawPaczke();
    public abstract void NadajPaczke();
}
