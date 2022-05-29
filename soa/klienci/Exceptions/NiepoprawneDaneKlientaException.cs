using inpost;
using System.Text.Json;

namespace klienci.Exceptions;

public class NiepoprawneDaneKlientaException : Exception
{
    public NiepoprawneDaneKlientaException(Klient klient, JsonSerializerOptions? jsonSerializerOptions = null) : base(
            "Podano niepoprawne dane klienta: " +
            JsonSerializer.Serialize<Klient>(klient)
        ) { }

    public NiepoprawneDaneKlientaException(int id, Klient klient, JsonSerializerOptions? jsonSerializerOptions = null) : base(
            $"Podano niepoprawne dane klienta (id = {id}): " +
            JsonSerializer.Serialize<Klient>(klient)
        ) { }
}
