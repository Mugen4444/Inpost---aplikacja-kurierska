internal class TerminalBlik : PlatnoscBlik
{
    private Terminal terminal;

    public TerminalBlik(Terminal terminal) : base(UslugaPlatnosci.GetInstance(terminal.Banki))
    {
        this.terminal = terminal;
    }

    public override void DokonajPlatnoscBlik()
    {
        var uslugaBlik = usluga.Blik;

        var kodBlik = WprowadzKodBlik();
        
        var karta = uslugaBlik.OtrzymajKarte(kodBlik);
        terminal.DokonajPlatnosc(karta);
    }
}
