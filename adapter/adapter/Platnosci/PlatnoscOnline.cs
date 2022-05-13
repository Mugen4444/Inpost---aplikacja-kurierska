using System.Text.RegularExpressions;

internal class PlatnoscOnline : PlatnoscBlik
{
    public PlatnoscOnline(IEnumerable<Bank> banki) : base(UslugaPlatnosci.GetInstance(banki))
    {

    }

    private bool SprawdzNrKarty(string nrKarty)
    {
        return nrKarty.Length == 16 && !new Regex("\\D").IsMatch(nrKarty);
    }

    private bool SprawdzCVV(string kodCvv)
    {
        try
        {
            var cvv = int.Parse(kodCvv);
            return cvv > 99 && cvv < 1000;
        }
        catch
        {
            return false;
        }
    }
     
    private string WprowadzNrKarty()
    {
        Console.Write("Wprowadź numer karty: ");

        var nrKarty = (Console.ReadLine()!).Trim().Replace(" ", "");
        ArgumentNullException.ThrowIfNull(nrKarty);

        Console.WriteLine("");

        while (!SprawdzNrKarty(nrKarty))
        {
            Console.WriteLine("Niepoprawny numer karty. Spróbuj ponownie.");
            Console.Write("Wprowadź numer karty: ");

            nrKarty = (Console.ReadLine()!).Trim().Replace(" ", "");
            ArgumentNullException.ThrowIfNull(nrKarty);

            Console.WriteLine("");
        }

        return nrKarty;
    }

    private int WprowadzCVV()
    {
        Console.Write("Wprowadź kod CVV: ");

        var kodCvv = (Console.ReadLine()!).Trim().Replace(" ", "");
        ArgumentNullException.ThrowIfNull(kodCvv);

        Console.WriteLine("");

        while (!SprawdzCVV(kodCvv))
        {
            Console.WriteLine("Niepoprawny kod CVV. Spróbuj ponownie.");
            Console.Write("Wprowadź kod CVV: ");

            kodCvv = (Console.ReadLine()!).Trim().Replace(" ", "");
            ArgumentNullException.ThrowIfNull(kodCvv);

            Console.WriteLine("");
        }

        return int.Parse(kodCvv);
    }

    public void DokonajPlatnoscOnline()
    {
        Console.WriteLine("Wprowadź następujące dane, żeby dokonać płatność:");

        var nrKarty = WprowadzNrKarty();
        var cvv = WprowadzCVV();

        var done = false;

        while (!done)
        {
            try
            {
                usluga.DokonajPlatnosc(nrKarty, cvv);
                done = true;
                Console.WriteLine("Płatność została dokonana pomyślnie");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
