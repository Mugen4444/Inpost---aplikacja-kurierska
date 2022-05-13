internal record Karta(Bank Bank, string Numer, int CVV)
{
    public string UkryjNumer()
    {
        return UkryjNumer(Numer);
    }

    public static string UkryjNumer(string numerKarty)
    {
        return (
            numerKarty.Substring(0, 4) +
            numerKarty.Substring(4, numerKarty.Length - 8).Select(_ => "*").Aggregate((acc, val) => acc + val) +
            numerKarty.Substring(numerKarty.Length - 4, 4)
        );
    }

    public override string ToString()
    {
        return $"Karta banku {Bank}: {UkryjNumer()}";
    }
}