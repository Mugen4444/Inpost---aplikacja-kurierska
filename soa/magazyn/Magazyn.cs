using inpost;

using magazyn.Exceptions;
using System.Text.Json;

using Microsoft.Extensions.Options;
using JsonOptions = Microsoft.AspNetCore.Http.Json.JsonOptions;

namespace magazyn;

public class Magazyn
{
    private readonly List<Paczka> paczki;
    private readonly IHttpClientFactory _clientFactory;
    private readonly JsonSerializerOptions jsonSerializerOptions;

    public Magazyn(IHttpClientFactory clientFactory, IOptions<JsonOptions> jsonOptions)
    {
        paczki = new List<Paczka>();
        _clientFactory = clientFactory;
        jsonSerializerOptions = jsonOptions.Value.SerializerOptions;
    }

    // Create
    public async Task<Paczka> DodajPaczke(Paczka paczka)
    {
        if (!Valid(paczka))
            throw new NiepoprawneDanePaczkiException(paczka, jsonSerializerOptions);

        paczka.Id = paczki.Count;

        while (paczki.Any(p => p.Id == paczka.Id))
        {
            paczka.Id++;
        }

        var uzupelniona = await UzupelnijNadawceIOdbiorce(paczka);

        if (!Valid(uzupelniona))
            throw new PaczkaOperationException((int)(uzupelniona!.Id!), "dodanie");

        paczki.Add(uzupelniona);

        return uzupelniona;
    }

    // Read
    public IEnumerable<Paczka> Wszystkie => paczki;

    // Read (id)
    public Paczka Paczka(int id)
    {
        var paczka = paczki.Find(p => p.Id == id);

        if (paczka is null)
            throw new NieZnalezionoPaczkiException(id);

        return paczka;
    }

    // Update
    public async Task<Paczka> OdswiezPaczke(int id, Paczka odswiezona)
    {
        if (!Valid(odswiezona))
            throw new NiepoprawneDanePaczkiException(odswiezona, jsonSerializerOptions);

        var paczka = paczki.Find(p => p.Id == id);

        if (paczka is null)
            throw new NieZnalezionoPaczkiException(id);

        odswiezona.Id = paczka.Id;

        var uzupelniona = await UzupelnijNadawceIOdbiorce(odswiezona);

        if (!Valid(uzupelniona))
            throw new PaczkaOperationException(id, "odświezenie");

        if (!paczki.Remove(paczka))
            throw new PaczkaOperationException(id, "odświeżenie");

        paczki.Add(uzupelniona);

        return uzupelniona;
    }

    // Delete
    public void UsunPaczke(int id)
    {
        var paczka = paczki.Find(p => p.Id == id);

        if (paczka is null)
            throw new NieZnalezionoPaczkiException(id);

        if (!paczki.Remove(paczka))
        {
            throw new PaczkaOperationException(id, "usunięcie");
        }
    }

    private bool Valid(Paczka paczka)
    {
        return !(
                string.IsNullOrEmpty(paczka.Zawartosc) ||
                paczka.Nadawca is null ||
                paczka.Odbiorca is null ||
                paczka.DataDodania < DateTime.Now.Subtract(TimeSpan.FromDays(1))
            );
    }

    // Reference to another service
    private async Task<Paczka> UzupelnijNadawceIOdbiorce(Paczka paczka)
    {
        var nadawca = await WyszukajKlienta(paczka.Nadawca!);
        var odbiorca = await WyszukajKlienta(paczka.Odbiorca!);

        if (nadawca is not null)
            paczka.Nadawca = nadawca;
        else
            paczka.Nadawca = await DodajKlienta(paczka.Nadawca!);


        if (odbiorca is not null)
            paczka.Odbiorca = odbiorca;
        else
            paczka.Odbiorca = await DodajKlienta(paczka.Odbiorca!);

        return paczka;
    }

    // Klienci service hooks

    private async Task<Klient?> WyszukajKlienta(Klient klient)
    {
        // Create klienci HttpClient

        var httpClient = _clientFactory.CreateClient("Klienci");

        var jsonStream = JsonContent.Create<Klient>(klient, options: jsonSerializerOptions);

        var request = new HttpRequestMessage()
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(httpClient!.BaseAddress!, "klienci"),
            Content = jsonStream
        };

        var response = await httpClient!.SendAsync(request);

        if (response.IsSuccessStatusCode)
        {
            var contentStream = await response.Content.ReadAsStreamAsync();

            var k = await JsonSerializer.DeserializeAsync<Klient>(contentStream, jsonSerializerOptions);

            if (k is null)
                throw new ApplicationException("Nie udało się przetworzyć dane klienta");

            return k;
        }

        return null;
    }

    private async Task<Klient?> DodajKlienta(Klient klient)
    {
        // Create klienci HttpClient

        var httpClient = _clientFactory.CreateClient("Klienci");

        var jsonStream = JsonContent.Create<Klient>(klient, options: jsonSerializerOptions);

        var response = await httpClient!.PostAsync("klienci", jsonStream);

        if (response.IsSuccessStatusCode)
        {
            var contentStream = await response.Content.ReadAsStreamAsync();

            var k = await JsonSerializer.DeserializeAsync<Klient>(contentStream, jsonSerializerOptions);

            if (k is null)
                throw new ApplicationException("Nie udało się przetworzyć dane klienta");

            return k;
        }

        return null;
    }
}
