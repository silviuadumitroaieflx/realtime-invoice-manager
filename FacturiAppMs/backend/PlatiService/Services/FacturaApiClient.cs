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

    public async Task<FacturaDto?> GetFacturaByNr(int nrFactura)
    {
        try
        {
            // FacturaService expune GET /Factura/GetFacturaByNr?nrFactura={nrFactura}
            return await _http.GetFromJsonAsync<FacturaDto>($"/Factura/GetFacturaByNr?nrFactura={nrFactura}");
        }
        catch
        {
            return null;
        }
    }
}
