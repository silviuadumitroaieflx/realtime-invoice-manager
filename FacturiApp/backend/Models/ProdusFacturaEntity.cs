namespace FacturiApp.Models;

public class ProdusFacturaEntity
{
    public int Id { get; set; }
    public int NrFactura { get; set; }
    public FacturaEntity? Factura { get; set; }
    public int IdProdus { get; set; }
    public string? Denumire { get; set; }
    public decimal PretUnitar { get; set; }
    public int Cantitate { get; set; }

}
