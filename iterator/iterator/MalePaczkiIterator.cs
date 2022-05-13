internal class MalePaczkiIterator : PaczkiIterator
{
    public MalePaczkiIterator(Magazyn magazyn) : base(magazyn)
    {
    }

    public override Paczka NastepnaPaczka()
    {
        var paczka = base.NastepnaPaczka();

        while (paczka is not PaczkaMala && SaPaczki())
        {
            paczka = base.NastepnaPaczka();
        }

        if (paczka is not PaczkaMala)
        {
            throw new Exception("Nie ma wiecej malych paczek");
        }

        return paczka;
    }
}
