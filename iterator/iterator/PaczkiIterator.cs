internal class PaczkiIterator
{
    private Magazyn magazyn;
    private int pozycja;

    public PaczkiIterator(Magazyn magazyn)
    {
        this.magazyn = magazyn;
        pozycja = -1;
    }

    public bool SaPaczki()
    {
        return pozycja < magazyn.Liczba - 1;
    }

    public virtual Paczka NastepnaPaczka()
    {
        if (SaPaczki())
        {
            pozycja++;
            return magazyn[pozycja];
        }

        throw new IndexOutOfRangeException("Nie ma wiecej paczek");
    }

    public void NaPoczatek()
    {
        pozycja = -1;
    }
}
