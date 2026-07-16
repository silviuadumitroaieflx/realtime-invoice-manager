using FacturaService.Data;
using FacturaService.Models;
using FacturaService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FacturaService.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class FacturaController : ControllerBase
{
    private readonly FacturaDbContext _db;
    private readonly ClientApiClient _clientApi;

    public FacturaController(FacturaDbContext db, ClientApiClient clientApi)
    {
        _db = db;
        _clientApi = clientApi;
    }

    [HttpGet]
    public async Task<ActionResult<List<FacturaEntity>>> GetFacturi()
    {
        var lista = await _db.Facturi
            .Include(f => f.ProduseFactura)
            .ToListAsync();

        foreach (var factura in lista)
            factura.Client = await _clientApi.GetClientById(factura.IdClient);

        return Ok(lista);
    }

    [HttpGet]
    public async Task<ActionResult<FacturaEntity>> GetFacturaByNr(int nrFactura)
    {
        var factura = await _db.Facturi
            .Include(f => f.ProduseFactura)
            .FirstOrDefaultAsync(f => f.NrFactura == nrFactura);

        if (factura == null)
            return NotFound($"Factura cu numarul {nrFactura} nu a fost gasita.");

        factura.Client = await _clientApi.GetClientById(factura.IdClient);
        return Ok(factura);
    }

    [HttpGet]
    public async Task<ActionResult<List<FacturaEntity>>> GetFacturiByClient(int idClient)
    {
        var facturi = await _db.Facturi
            .Include(f => f.ProduseFactura)
            .Where(f => f.IdClient == idClient)
            .ToListAsync();

        if (facturi.Count == 0)
            return NotFound($"Nu s-au gasit facturi pentru clientul cu id-ul {idClient}.");

        foreach (var factura in facturi)
            factura.Client = await _clientApi.GetClientById(factura.IdClient);

        return Ok(facturi);
    }

    [HttpPost]
    public async Task<ActionResult> AdaugaFactura([FromBody] FacturaEntity factura)
    {
        _db.Facturi.Add(factura);
        await _db.SaveChangesAsync();
        return Ok($"Factura adaugata cu succes! Numar: {factura.NrFactura}");
    }

    [HttpPut]
    public async Task<ActionResult> UpdateFactura([FromQuery] int nrFactura, [FromBody] FacturaEntity facturaActualizata)
    {
        var factura = await _db.Facturi
            .Include(f => f.ProduseFactura)
            .FirstOrDefaultAsync(f => f.NrFactura == nrFactura);

        if (factura == null)
            return NotFound($"Factura cu numarul {nrFactura} nu a fost gasita.");

        factura.DataFactura = facturaActualizata.DataFactura;
        factura.IdClient = facturaActualizata.IdClient;
        factura.Observatii = facturaActualizata.Observatii;

        _db.ProdusFactura.RemoveRange(factura.ProduseFactura);
        factura.ProduseFactura = facturaActualizata.ProduseFactura;

        await _db.SaveChangesAsync();
        return Ok("Factura actualizata cu succes!");
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteFactura(int nrFactura)
    {
        var factura = await _db.Facturi.FindAsync(nrFactura);
        if (factura == null)
            return NotFound($"Factura cu numarul {nrFactura} nu a fost gasita.");

        _db.Facturi.Remove(factura);
        await _db.SaveChangesAsync();
        return Ok("Factura stearsa cu succes!");
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteProdusDinFactura(int nrFactura, int idProdusFactura)
    {
        var linie = await _db.ProdusFactura
            .FirstOrDefaultAsync(pf => pf.NrFactura == nrFactura && pf.Id == idProdusFactura);

        if (linie == null)
            return NotFound($"Linia cu id {idProdusFactura} din factura {nrFactura} nu a fost gasita.");

        _db.ProdusFactura.Remove(linie);
        await _db.SaveChangesAsync();
        return Ok("Produs sters din factura cu succes!");
    }
}
