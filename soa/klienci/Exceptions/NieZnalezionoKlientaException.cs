using inpost;
using System.Text.Json;

namespace klienci.Exceptions;

public class NieZnalezionoKlientaException : Exception
{
    public NieZnalezionoKlientaException(int id) : base(
            $"Nie znaleziono klienta id: {id}"
        ) { }

    public NieZnalezionoKlientaException(Klient klient, JsonSerializerOptions? jsonSerializerOptions = null) : base(
            $"Nie znaleziono klienta: " +
            JsonSerializer.Serialize<Klient>(klient, jsonSerializerOptions)
        ) { }
}
