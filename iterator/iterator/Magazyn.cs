internal class Magazyn
{
    private Paczka[] paczki;

    public Magazyn()
    {
        paczki = new Paczka[20];

        for (var i = 0; i < paczki.Length; i++)
        {
            var opcja = new Random().Next(0, 3);

            switch (opcja)
            {
                case 0:
                    paczki[i] = new PaczkaMala();
                    break;
                case 1:
                    paczki[i] = new PaczkaSrednia();
                    break;
                case 2:
                    paczki[i] = new PaczkaDuza();
                    break;
                default:
                    throw new ApplicationException("Wystapil blad");
            }
        }

    }

    public Paczka this[int index]
    {
        get
        {
            return paczki[index];
        }
    }

    public PaczkiIterator GetIterator()
    {
        return new PaczkiIterator(this);
    }

    public PaczkiIterator GetMalePaczkiIterator()
    {
        return new MalePaczkiIterator(this);
    }

    public int Liczba { get => paczki.Length; }
}
