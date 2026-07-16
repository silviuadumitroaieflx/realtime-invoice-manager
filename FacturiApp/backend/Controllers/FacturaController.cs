using FacturiApp.Data;
using FacturiApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FacturiApp.Controllers;

[ApiController]
[Authorize]
[Route("[controller]/[action]")]
public class FacturaController : ControllerBase
{
    private readonly AppDbContext _db;

    public FacturaController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<List<FacturaEntity>>> GetFacturi()
    {
        var lista = await _db.Facturi
            .Include(f => f.Client)
            .Include(f => f.ProduseFactura)
            .ToListAsync();
        return Ok(lista);
    }

    [HttpGet]
    public async Task<ActionResult<FacturaEntity>> GetFacturaByNr(int nrFactura)
    {
        var factura = await _db.Facturi
            .Include(f => f.Client)
            .Include(f => f.ProduseFactura)
            .FirstOrDefaultAsync(f => f.NrFactura == nrFactura);

        if (factura == null)
            return NotFound($"Factura cu numarul {nrFactura} nu a fost gasita.");
        return Ok(factura);
    }

    [HttpGet]
    public async Task<ActionResult<List<FacturaEntity>>> GetFacturiByClient(int idClient)
    {
        var facturi = await _db.Facturi
            .Include(f => f.Client)
            .Include(f => f.ProduseFactura)
            .Where(f => f.IdClient == idClient)
            .ToListAsync();

        if (facturi.Count == 0)
            return NotFound($"Nu s-au gasit facturi pentru clientul cu id-ul {idClient}.");
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
