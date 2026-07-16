using FacturaService.Models;

namespace FacturaService.Services;

public class ClientApiClient
{
    private readonly HttpClient _http;

    public ClientApiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<ClientDto?> GetClientById(int id, string? authHeader)
    {
        try
        {
            // ClientService expune GET /Client/GetClientById?id={id}
            var cerere = new HttpRequestMessage(HttpMethod.Get, $"/Client/GetClientById?id={id}");
            if (!string.IsNullOrEmpty(authHeader))
                cerere.Headers.TryAddWithoutValidation("Authorization", authHeader);

            var raspuns = await _http.SendAsync(cerere);
            if (!raspuns.IsSuccessStatusCode)
                return null;

            return await raspuns.Content.ReadFromJsonAsync<ClientDto>();
        }
        catch
        {
            // Daca ClientService nu raspunde sau clientul nu exista,
            // factura ramane cu Client = null
            return null;
        }
    }
}
