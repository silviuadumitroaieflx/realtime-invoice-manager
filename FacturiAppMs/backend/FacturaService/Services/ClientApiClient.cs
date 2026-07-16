using FacturaService.Models;

namespace FacturaService.Services;

public class ClientApiClient
{
    private readonly HttpClient _http;

    public ClientApiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<ClientDto?> GetClientById(int id)
    {
        try
        {
            // ClientService expune GET /Client/GetClientById?id={id}
            return await _http.GetFromJsonAsync<ClientDto>($"/Client/GetClientById?id={id}");
        }
        catch
        {
            // Daca ClientService nu raspunde sau clientul nu exista,
            // factura ramane cu Client = null
            return null;
        }
    }
}
