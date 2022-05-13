internal record Paczka
{
    public string Zawartosc { get; }

    public Paczka(string Zawartosc)
    {
        if (Zawartosc.Length > 0)
        {
            this.Zawartosc = Zawartosc;
        }
        else
        {
            throw new ApplicationException("Paczka nie moze byc pusta");
        }
    }
}