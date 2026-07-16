namespace PlatiService.Models;

// Copie a datelor de factura aduse prin HTTP din FacturaService.
public class FacturaDto
{
    public int NrFactura { get; set; }
    public int IdClient { get; set; }
    public DateTime DataFactura { get; set; }
    public string? Observatii { get; set; }
}
