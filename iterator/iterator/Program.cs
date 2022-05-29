var magazyn = new Magazyn();
var malePaczki = magazyn.GetMalePaczkiIterator();
var pierwszy= magazyn[0];

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
