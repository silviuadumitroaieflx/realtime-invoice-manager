using PlatiService.Models;

namespace PlatiService.Services;

// Client HTTP catre FacturaService. URL din appsettings.json -> "Services:FacturaService".
public class FacturaApiClient
{
    private readonly HttpClient _http;

    public FacturaApiClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<FacturaDto?> GetFacturaByNr(int nrFactura, string? authHeader)
    {
        try
        {
            // FacturaService expune GET /Factura/GetFacturaByNr?nrFactura={nrFactura}
            var cerere = new HttpRequestMessage(HttpMethod.Get, $"/Factura/GetFacturaByNr?nrFactura={nrFactura}");
            if (!string.IsNullOrEmpty(authHeader))
                cerere.Headers.TryAddWithoutValidation("Authorization", authHeader);

            var raspuns = await _http.SendAsync(cerere);
            if (!raspuns.IsSuccessStatusCode)
                return null;

            return await raspuns.Content.ReadFromJsonAsync<FacturaDto>();
        }
        catch
        {
            return null;
        }
    }
}
