using System.Text.RegularExpressions;

internal abstract class Bank
{
    private string nazwa;
    private int kod;
    public int Kod { get => kod; }

    private IList<Karta> karty;
    private UslugaBlik blik;

    public Bank(UslugaBlik uslugaBlik, string nazwaBanku, int kodBanku)
    {
        SprawdzBank(nazwaBanku, kodBanku);

        nazwa = nazwaBanku;
        kod = kodBanku;
        karty = new List<Karta>();
        blik = uslugaBlik;
    }

    private string WygenerujNrKarty()
    {
        var unique = false;
        var numerKarty = "";
        while (!unique)
        {
            numerKarty = new List<int>() { kod }.Concat(
                new List<int>(
                    Enumerable.Repeat(0, 3)
                )
                .Select(_ => new Random().Next(0, 10000))
            )
            .Select(k => k.ToString("D4"))
            .Aggregate((a, b) => a + b);

            unique = !karty.Any(karty => numerKarty.Equals(numerKarty));
        }

        return numerKarty;
    }

    private int WygenerujCVV()
    {
        int cvv = (
            new Random().Next(0, 10) +
            new Random().Next(0, 10) * 10 +
            new Random().Next(0, 10) * 100
            );

        return cvv;
    }

    public Karta OtworzKonto()
    {
        var nrKarty = WygenerujNrKarty();

        var cvv = WygenerujCVV();

        var karta = new Karta(this, nrKarty, cvv);

        karty.Add(karta);

        return karta;
    }

    private void Sprawdz(Karta karta)
    {
        var k = karty.Where(k => k.Numer.Equals(karta.Numer));

        if (k.Any())
        {
            if (k.First().CVV != karta.CVV)
            {
                throw new ApplicationException($"Niepoprawny CVV dla karty: {karta.UkryjNumer()}");
            }
        }
        else
        {
            throw new ApplicationException($"Ta karta nie jest obsługiwana w banku {this}: {karta.UkryjNumer()}");
        }
    }

    private void Sprawdz(string numerKarty, int cvv)
    {
        if (numerKarty.Length != 16)
        {
            throw new InvalidDataException("Numer karty musi składać się z 16 cyfr.");
        }

        if (new Regex("\\D").IsMatch(numerKarty))
        {
            throw new InvalidDataException("Numer karty musi składać się wyłącznie z cyfr.");
        }

        if (cvv < 100 || cvv > 999)
        {
            throw new InvalidDataException("Numer cvv musi składać się z 3 cyfr.");
        }

        var karta = karty.Where(k => k.Numer.Equals(numerKarty));

        if (karta.Any())
        {
            if (karta.First().CVV != cvv)
            {
                throw new ApplicationException($"Niepoprawny CVV dla karty: {Karta.UkryjNumer(numerKarty)}");
            }
        }
        else
        {
            throw new ApplicationException($"Ta karta nie jest obsługiwana w banku {this}: {Karta.UkryjNumer(numerKarty)}");
        }
    }

    public void Sprawdz(int kodBlik)
    {
        Sprawdz(blik.OtrzymajKarte(kodBlik));
    }

    public void WygenerujBlik(Karta karta)
    {
        Sprawdz(karta);

        blik.WygenerujBlik(karta);
    }

    public void WygenerujBlik(string numerKarty, int cvv)
    {
        Sprawdz(numerKarty, cvv);

        var karta = karty.Where(k => k.Numer.Equals(numerKarty)).First();

        blik.WygenerujBlik(karta);
    }

    public void Platnosc(int kodBlik)
    {
        var karta = blik.OtrzymajKarte(kodBlik);

        Console.WriteLine($"Platność BLIK dokonana w banku {this}: {karta.UkryjNumer()}");

        blik.UniewaznijKod(kodBlik);
    }

    public void Platnosc(Karta karta)
    {
        Sprawdz(karta);

        Console.WriteLine($"Platność dokonana w banku {this}: {karta.UkryjNumer()}");
    }

    public void Platnosc(string numerKarty, int cvv)
    {
        Sprawdz(numerKarty, cvv);

        Console.WriteLine($"Platność dokonana w banku {this}: {Karta.UkryjNumer(numerKarty)}");
    }

    private void SprawdzBank(string nazwaBanku, int kod)
    {
        if (!"millenium, santander, pko".Contains(nazwaBanku.ToLower().Trim().Replace(" ", "")))
        {
            throw new InvalidDataException($"Nie istnieje takiego banku: {nazwaBanku}");
        }

        if (kod > 9999 || kod < 0)
        {
            throw new InvalidDataException($"Niepoprawny kod banku {kod:0000}");
        }
    }

    public override string ToString()
    {
        return nazwa;
    }
}
