namespace magazyn.Exceptions;

public class PaczkaOperationException : Exception
{
    public PaczkaOperationException(int id, string operacja) : base(
            $"Nie udało się dokonać operacji {operacja.ToUpper()} nad Paczką id: {id}"
        ) { }
}
