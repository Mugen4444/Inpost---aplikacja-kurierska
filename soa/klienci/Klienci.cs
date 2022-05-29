using inpost;
using klienci.Exceptions;
using System.Text.RegularExpressions;

using Microsoft.Extensions.Options;
using System.Text.Json;
using JsonOptions = Microsoft.AspNetCore.Http.Json.JsonOptions;

namespace klienci;

public class Klienci
{
    private readonly List<Klient> klienci;
    private readonly JsonSerializerOptions jsonSerializerOptions;

    public Klienci(IOptions<JsonOptions> jsonOptions)
    {
        klienci = new List<Klient>();
        jsonSerializerOptions = jsonOptions.Value.SerializerOptions;
    }

    // Create
    public Klient DodajKlienta(Klient klient)
    {
        if (!Valid(klient))
            throw new NiepoprawneDaneKlientaException(klient, jsonSerializerOptions);

        klient.Id = klienci.Count;

        while (klienci.Any(k => k.Id == klient.Id))
        {
            klient.Id++;
        }

        klienci.Add(klient);

        return klient;
    }

    // Read
    public IEnumerable<Klient> Wszystkie => klienci;

    // Read (id)
    public Klient Klient(int id)
    {
        var klient = klienci.Find(k => k.Id == id);

        if (klient is null)
            throw new NieZnalezionoKlientaException(id);

        return klient;
    }

    // Read (Klient)
    public Klient Klient(Klient klient)
    {
        if (!Valid(klient))
            throw new NiepoprawneDaneKlientaException(klient, jsonSerializerOptions);

        var znaleziono = klienci.Find(k => k.Equals(klient));

        if (znaleziono is null)
            throw new NieZnalezionoKlientaException(klient, jsonSerializerOptions);

        return znaleziono;
    }

    // Update
    public Klient OdswiezKlienta(int id, Klient odswiezony)
    {
        if (!Valid(odswiezony))
            throw new NiepoprawneDaneKlientaException(id, odswiezony, jsonSerializerOptions);

        var klient = klienci.Find(k => k.Id == id);

        if (klient is null)
            throw new NieZnalezionoKlientaException(id);

        odswiezony.Id = klient.Id;

        if (!klienci.Remove(klient))
        {
            throw new KlientOperationException(id, "odświeżenie");
        }

        klienci.Add(odswiezony);

        return klient;
    }

    // Delete
    public void UsunKlienta(int id)
    {
        var klient = klienci.Find(k => k.Id == id);

        if (klient is null)
            throw new NieZnalezionoKlientaException(id);

        if (!klienci.Remove(klient))
        {
            throw new KlientOperationException(id, "usunięcie");
        }
    }

    // Validate
    private bool Valid(Klient klient)
    {
        return !(
                string.IsNullOrEmpty(klient.Imie) ||
                string.IsNullOrEmpty(klient.Nazwisko) ||
                string.IsNullOrEmpty(klient.NrTelefonu) ||
                string.IsNullOrEmpty(klient.AdresZamieszkania) ||
                new Regex(@"[^\d\+]").IsMatch(klient.NrTelefonu)
            );
    }
}
