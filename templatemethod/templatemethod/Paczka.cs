internal class Paczka
{
    private object zawartosc;
    public int Id { get; } = new Random().Next();
    public object Zawartosc { get => zawartosc; }

    public Paczka(object zawartosc)
    {
        this.zawartosc = zawartosc;
    }
}
