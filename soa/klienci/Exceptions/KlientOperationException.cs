namespace klienci.Exceptions;

public class KlientOperationException : Exception
{
    public KlientOperationException(int id, string operacja) : base(
            $"Nie udało się dokonać operacji {operacja.ToUpper()} nad klientem id: {id}"
        ) { }
}
