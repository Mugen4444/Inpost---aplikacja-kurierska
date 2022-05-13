Bank millenium = new Millenium();
Bank pko = new PKO();
Bank santander = new Santander();

IEnumerable<Bank> banki = new List<Bank>()
{
    millenium,
    pko,
    santander
};

var karta = millenium.OtworzKonto();
Console.WriteLine($"Utworzono konto w banku {karta.Bank.ToString()} z numerem {karta.Numer} oraz CVV {karta.CVV}");

var aplikacja = new Aplikacja(karta);

var online = new PlatnoscOnline(banki);
var terminal = new Terminal(banki);
var blik = new TerminalBlik(terminal);

aplikacja.OtrzymajKodBlik();
blik.DokonajPlatnoscBlik();

terminal.DokonajPlatnosc(karta);

online.DokonajPlatnoscOnline();