using inpost;
using System.Text.Json;

namespace magazyn.Exceptions;

public class NiepoprawneDanePaczkiException : Exception
{
    public NiepoprawneDanePaczkiException(Paczka paczka, JsonSerializerOptions? jsonSerializerOptions = null) : base(
            "Podano niepoprawne dane paczki: " +
            JsonSerializer.Serialize<Paczka>(paczka, jsonSerializerOptions)
        ) { }

    public NiepoprawneDanePaczkiException(int id, Paczka paczka, JsonSerializerOptions? jsonSerializerOptions = null) : base(
            $"Podano niepoprawne dane paczki (id = {id}): " +
            JsonSerializer.Serialize<Paczka>(paczka, jsonSerializerOptions)
        ) { }
}
