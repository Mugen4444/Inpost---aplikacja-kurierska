internal class UslugaBlik
{
    private IDictionary<int, Karta> aktywneBliki;

    private static readonly object _lock = new object();
    private static UslugaBlik? instance;

    private UslugaBlik()
    {
        aktywneBliki = new Dictionary<int, Karta>();
    }

    public static UslugaBlik Instance
    {
        get
        {
            if (instance is null)
            {
                lock (_lock)
                {
                    if (instance is null)
                    {
                        instance = new UslugaBlik();
                    }
                }
            }

            return instance;
        }
    }

    public void WygenerujBlik(Karta karta)
    {
        var unique = false;

        var blik = (
            new Random().Next(0, 10) +
            new Random().Next(0, 10) * 10 +
            new Random().Next(0, 10) * 100 +
            new Random().Next(0, 10) * 1000 +
            new Random().Next(0, 10) * 10000 +
            new Random().Next(0, 10) * 100000
        );

        unique = !aktywneBliki.ContainsKey(blik);

        while (!unique)
        {
            blik = (
                new Random().Next(0, 10) +
                new Random().Next(0, 10) * 10 +
                new Random().Next(0, 10) * 100 +
                new Random().Next(0, 10) * 1000 +
                new Random().Next(0, 10) * 10000 +
                new Random().Next(0, 10) * 100000
            );

            unique = aktywneBliki.ContainsKey(blik);
        }

        aktywneBliki.Add(new KeyValuePair<int, Karta>(blik, karta));

        Console.WriteLine($"Wprowadź ten kod: {blik.ToString("D6")}");
    }

    private void SprawdzBlik(int blik)
    {
        if (!aktywneBliki.ContainsKey(blik))
        {
            UniewaznijKod(blik);
            throw new ApplicationException("Niepoprawny kod blik");
        }
    }

    public void UniewaznijKod(int blik)
    {
        SprawdzBlik(blik);

        if (aktywneBliki.ContainsKey(blik))
        {
            aktywneBliki.Remove(blik);
        }
    }

    public Karta OtrzymajKarte(int blik)
    {
        SprawdzBlik(blik);

        return aktywneBliki[blik];
    }
}
