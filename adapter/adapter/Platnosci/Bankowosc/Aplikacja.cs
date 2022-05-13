internal class Aplikacja
{
    private Karta karta;

    public Aplikacja(Karta karta)
    {
        this.karta = karta;
    }

    public void OtrzymajKodBlik()
    {
        karta.Bank.WygenerujBlik(karta);
    }
}
