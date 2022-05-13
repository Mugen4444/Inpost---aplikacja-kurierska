using System.Text.RegularExpressions;

internal abstract class PlatnoscBlik
{
    protected UslugaPlatnosci usluga;

    public PlatnoscBlik(UslugaPlatnosci uslugaPlatnosci)
    {
        usluga = uslugaPlatnosci;
    }

    private bool SprawdzBlik(string blik)
    {
        return blik.Length == 6 && !(new Regex("\\D").IsMatch(blik));
    }

    protected int WprowadzKodBlik()
    {
        Console.Write("Żeby dokonać płatność, wprowadź kod blik: ");

        var blik = (Console.ReadLine()!).Trim().Replace(" ", "");
        ArgumentNullException.ThrowIfNull(blik);

        Console.WriteLine("");

        while (!SprawdzBlik(blik))
        {
            Console.WriteLine("Niepoprawny kod blik. Spróbuj ponownie.");
            Console.Write("Żeby dokonać płatność, wprowadź kod blik: ");

            blik = (Console.ReadLine()!).Trim().Replace(" ", "");
            ArgumentNullException.ThrowIfNull(blik);

            Console.WriteLine("");
        }

        return int.Parse(blik);
    }

    public virtual void DokonajPlatnoscBlik()
    {
        var done = false;

        while (!done)
        {
            var kodBlik = WprowadzKodBlik();

            try
            {
                usluga.DokonajPlatnosc(kodBlik);
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
