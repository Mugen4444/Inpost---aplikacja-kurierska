internal class Zamowienie
{
    private Paczka paczka;
    private Klient odbiorca;
    private Klient nadawca;

    public Zamowienie(Paczka paczka, Klient nadawca, Klient odbiorca)
    {
        this.paczka = paczka;
        this.nadawca = nadawca;
        this.odbiorca = odbiorca;

        Console.WriteLine($"Utworzono zamowienie na wyslanie paczki {paczka.Id}, nadawca {nadawca.ImieNazwisko} odbiorca {odbiorca.ImieNazwisko}");
    }

    public Paczka Paczka { get => paczka; }
    public Klient Odbiorca { get => odbiorca; }
}