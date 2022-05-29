namespace magazyn.Exceptions;

public class NieZnalezionoPaczkiException : Exception
{
    public NieZnalezionoPaczkiException(int id) : base(
            $"Nie znaleziono Paczki id: {id}"
        )
    { }
}
