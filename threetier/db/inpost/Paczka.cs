internal record Paczka
{
    public int Id { get; }
    public string Zawartosc { get; }

    public Paczka(int Id, string Zawartosc)
    {
        if (!IsValidId(Id))
        {
            throw new ApplicationException("Niepoprawny id paczki");
        }
        else
        {
            this.Id = Id;
        }

        if (Zawartosc.Length > 0)
        {
            this.Zawartosc = Zawartosc;
        }
        else
        {
            throw new ApplicationException("Paczka nie moze byc pusta");
        }

    }

    public static bool IsValidId(int id)
    {
        return !(id < 0 || id > 9999);
    }

    public override string ToString()
    {
        return $"{Id} {Zawartosc}";
    }

    public static Paczka FromString(string dane)
    {
        var daneSpl = dane.Split(" ");

        var idStr = daneSpl[0];

        if (!IsValidId(int.Parse(idStr)))
        {
            throw new InvalidDataException("Niepoprawny Id paczki");
        }

        var zawartosc = "";

        for (var i = 1; i < daneSpl.Length; i++)
        {
            zawartosc += (i == 1 ? "" : " ") + daneSpl[i];
        }

        if (zawartosc.Length == 0)
        {
            throw new InvalidDataException("Paczka nie moze byc pusta");
        }

        return new Paczka(int.Parse(idStr), zawartosc);
    }
}