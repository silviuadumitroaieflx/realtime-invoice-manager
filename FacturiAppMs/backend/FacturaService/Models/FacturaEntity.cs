using System.ComponentModel.DataAnnotations.Schema;

namespace FacturaService.Models;

public class FacturaEntity
{
    public int IdClient { get; set; }
    public int NrFactura { get; set; }
    public DateTime DataFactura { get; set; } = DateTime.Now;
    public string? Observatii { get; set; }

    // In monolit venea prin Include(f => f.Client) din aceeasi baza.
    // Aici clientul sta in alt serviciu/alta baza, deci nu se mapeaza in DB;
    // se completeaza prin HTTP din ClientService (vezi FacturaController).
    [NotMapped]
    public ClientDto? Client { get; set; }

    public List<ProdusFacturaEntity> ProduseFactura { get; set; } = new();
}
