internal class Zamowienie
{
    private Paczka paczka;
    private Klient odbiorca;
    private Klient nadawca;

    public Zamowienie()
    {
        paczka = new Paczka();
        nadawca = new Klient();
        odbiorca = new Klient();

        Console.WriteLine($"Utworzono zamowienie na wyslanie paczki {paczka.Id}, nadawca {nadawca.ImieNazwisko} odbiorca {odbiorca.ImieNazwisko}");
    }
}