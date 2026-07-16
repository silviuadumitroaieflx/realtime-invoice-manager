namespace FacturaService.Models;

// Copie a datelor de client aduse prin HTTP din ClientService.
// Nu este stocata in baza FacturiFacturi (vezi [NotMapped] in FacturaEntity).
public class ClientDto
{
    public int IdClient { get; set; }
    public string? Email { get; set; }
    public string? Telefon { get; set; }
    public string? Adresa { get; set; }
    public string? NumeClient { get; set; }
}
