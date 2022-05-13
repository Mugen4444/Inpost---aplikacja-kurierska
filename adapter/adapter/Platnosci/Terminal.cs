internal class Terminal
{
    private UslugaPlatnosci usluga;
    public IEnumerable<Bank> Banki { get; }

    public Terminal(IEnumerable<Bank> banki)
    {
        Banki = banki;
        usluga = UslugaPlatnosci.GetInstance(banki);
    }

    public void DokonajPlatnosc(Karta karta)
    {
        var done = false;

        while (!done)
        {
            try
            {
                usluga.DokonajPlatnosc(karta);
                done = true;

                Console.WriteLine("Płatność została dokonana pomyślnie");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}