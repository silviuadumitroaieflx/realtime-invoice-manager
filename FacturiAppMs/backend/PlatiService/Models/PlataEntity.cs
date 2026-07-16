using System.ComponentModel.DataAnnotations.Schema;

namespace PlatiService.Models;

public class PlataEntity
{
    public int IdPlata { get; set; }
    public int NrFactura { get; set; }              // factura platita (din FacturaService)
    public decimal Suma { get; set; }
    public DateTime DataPlata { get; set; } = DateTime.Now;
    public string? MetodaPlata { get; set; }        // ex: numerar / card / transfer
    public string? Status { get; set; }             // ex: Platita / In asteptare / Anulata

    // Adusa prin HTTP din FacturaService (nu se stocheaza in baza FacturiPlati)
    [NotMapped]
    public FacturaDto? Factura { get; set; }
}
