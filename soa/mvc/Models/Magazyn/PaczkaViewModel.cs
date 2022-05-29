using inpost;
using System.Text.Json;
using Microsoft.Extensions.Options;
using JsonOptions = Microsoft.AspNetCore.Http.Json.JsonOptions;

namespace mvc.Models.Magazyn
{
    public class PaczkaViewModel
    {
        private readonly HttpClient httpClient;
        private readonly JsonSerializerOptions jsonSerializerOptions;

        public PaczkaViewModel(IHttpClientFactory clientFactory, IOptions<JsonOptions> jsonOptions)
        {
            if (clientFactory is null)
                throw new ApplicationException("Wystąpił błąd aplikacji");

            httpClient = clientFactory.CreateClient("Magazyn");
            jsonSerializerOptions = jsonOptions.Value.SerializerOptions;
        }

        public IEnumerable<Paczka> Paczki
        {
            get
            {
                var response = httpClient.GetAsync("paczki").Result;

                if (response.IsSuccessStatusCode)
                {
                    var contentStream = response.Content.ReadAsStream();
                    var paczki = JsonSerializer.Deserialize<IEnumerable<Paczka>>(contentStream, jsonSerializerOptions);

                    if (paczki is null)
                        throw new Exception("Nie udało się przetworzyć dane paczek");

                    return paczki;
                }

                var message = response.Content.ReadAsStringAsync().Result;

                throw new Exception("Wystąpił błąd przy połączeniu do usługi paczek: " + message);
            }
        }

        public void OdswiezPaczke(int id, Paczka odswiezona)
        {
            var jsonStream = JsonContent.Create<Paczka>(odswiezona, options: jsonSerializerOptions);

            var response = httpClient.PutAsync($"paczki/{id}", jsonStream).Result;

            if (response.IsSuccessStatusCode)
            {
                var contentStream = response.Content.ReadAsStream();
                var paczka = JsonSerializer.Deserialize<Paczka>(contentStream, jsonSerializerOptions);

                if (paczka is null)
                    throw new Exception("Nie udało się przetworzyć dane paczki");

                return;
            }

            var message = response.Content.ReadAsStringAsync().Result;

            throw new Exception("Wystąpił błąd przy połączeniu do usługi paczek: " + message);
        }

        public int Id { get; set; }

        public Paczka Paczka
        {
            get
            {
                var response = httpClient.GetAsync($"paczki/{Id}").Result;

                if (response.IsSuccessStatusCode)
                {
                    var contentStream = response.Content.ReadAsStream();
                    var paczka = JsonSerializer.Deserialize<Paczka>(contentStream, jsonSerializerOptions);

                    if (paczka is null)
                        throw new Exception("Nie udało się przetworzyć dane paczek");

                    return paczka;
                }

                var message = response.Content.ReadAsStringAsync().Result;

                throw new Exception("Wystąpił błąd przy połączeniu do usługi paczek: " + message);
            }
        }

        public void UsunPaczke()
        {
            var response = httpClient.DeleteAsync($"paczki/{Id}").Result;

            if (!response.IsSuccessStatusCode)
            {
                var message = response.Content.ReadAsStringAsync().Result;

                throw new Exception("Wystąpił błąd przy połączeniu do usługi paczek: " + message);
            }
        }

        public void WyslijPaczke(Paczka paczka)
        {
            var jsonStream = JsonContent.Create<Paczka>(paczka, options: jsonSerializerOptions);

            var response = httpClient.PostAsync($"paczki", jsonStream).Result;


            if (response.IsSuccessStatusCode)
            {
                var contentStream = response.Content.ReadAsStream();

                var p = JsonSerializer.Deserialize<Paczka>(contentStream, jsonSerializerOptions);

                if (p is null)
                    throw new Exception("Nie udało się przetworzyć dane paczki");

                return;
            }

            var message = response.Content.ReadAsStringAsync().Result;

            throw new Exception("Wystąpił błąd przy połączeniu do usługi paczek: " + message);
        }
    }
}
