var magazyn = new Magazyn();
var malePaczki = magazyn.GetMalePaczkiIterator();

while (malePaczki.SaPaczki())
{
    try
    {
        malePaczki.NastepnaPaczka().Informacje();
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}
