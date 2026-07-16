namespace FacturiApp.Models;

public class FacturaEntity
{
    public int IdClient { get; set; }
    public int NrFactura { get; set; }
    public DateTime DataFactura { get; set; } = DateTime.Now;
    public string? Observatii { get; set; }
    public ClientEntity? Client { get; set; }
    public List<ProdusFacturaEntity> ProduseFactura { get; set; } = new();

}
