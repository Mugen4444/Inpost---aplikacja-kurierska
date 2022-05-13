internal class UslugaPlatnosci
{
    private static readonly object _lock = new object();
    private static UslugaPlatnosci? instance;

    private IEnumerable<Bank> banki;
    private UslugaBlik blik;

    public UslugaBlik Blik { get => blik; }

    private UslugaPlatnosci(IEnumerable<Bank> banki)
    {
        this.banki = banki;
        blik = UslugaBlik.Instance;
    }

    public static UslugaPlatnosci GetInstance(IEnumerable<Bank> banki)
    {
        if (instance is null)
        {
            lock (_lock)
            {
                if (instance is null)
                {
                    instance = new UslugaPlatnosci(banki);
                }
            }
        }

        banki.ToList().ForEach(bank => {
            if (!instance.banki.Any(b => b.Equals(bank)))
            {
                instance.banki.Append(bank);
            }
        });

        return instance;
    }

    public void DokonajPlatnosc(string numerKarty, int cvv)
    {
        var bank = ZnajdzBank(numerKarty);
        bank.Platnosc(numerKarty, cvv);
    }

    public void DokonajPlatnosc(Karta karta)
    {
        var bank = ZnajdzBank(karta);
        bank.Platnosc(karta);
    }

    public void DokonajPlatnosc(int kodBlik)
    {
        var bank = banki.Aggregate((acc, val) =>
        {
            try
            {
                val.Sprawdz(kodBlik);
                return val;
            }
            catch
            {
                return acc;
            }
        });

        bank.Platnosc(kodBlik);
    }

    private Bank ZnajdzBank(string numerKarty)
    {
        var kod = int.Parse(numerKarty.Substring(0, 4));

        var bank = banki.Where(b => b.Kod == kod).FirstOrDefault();

        if (bank is null)
        {
            throw new ApplicationException($"Nie znaleziono banku, obsługującego tą kartę: {Karta.UkryjNumer(numerKarty)}");
        }

        return bank;
    }

    private Bank ZnajdzBank(Karta karta)
    {
        return ZnajdzBank(karta.Numer);
    }
}