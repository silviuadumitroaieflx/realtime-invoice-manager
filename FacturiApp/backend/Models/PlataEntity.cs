namespace FacturiApp.Models;

public class PlataEntity
{
    public int IdPlata { get; set; }
    public int NrFactura { get; set; }
    public decimal Suma { get; set; }
    public DateTime DataPlata { get; set; } = DateTime.Now;
    public string? MetodaPlata { get; set; }
    public string? Status { get; set; }
    public FacturaEntity? Factura { get; set; }
}
