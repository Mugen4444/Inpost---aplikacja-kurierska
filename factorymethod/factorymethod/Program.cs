while (true)
{
    Console.Write("Wybierz jednostkę (wydzial, kurier lub paczkomat): ");
    string nazwaJednostki = Console.ReadLine();

    Inpost jednostka = null;

    Console.WriteLine();
    try
    {
        jednostka = Inpost.Jednostka(nazwaJednostki);
    }
    catch (ArgumentException e)
    {
        Console.WriteLine("Przy otrzymaniu jednostki wystąpił błąd: " + e.Message);
        return;
    }

    Console.WriteLine("Wprowadź rozmiar paczki (mala, srednia lub duza): ");
    string rozmiar = Console.ReadLine();
    Console.WriteLine();
    Paczka paczka = null;

    try
    {
        paczka = jednostka.PrzygotujPaczke(rozmiar);
    }
    catch (ArgumentException e)
    {
        Console.WriteLine("Podczas przygotowania paczki wystąpił błąd: " + e.Message);
        return;
    }

    Console.WriteLine("Stworzono paczkę");
    paczka.Informacje();

    Console.ReadKey();
}