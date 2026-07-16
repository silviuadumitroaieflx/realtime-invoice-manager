using FacturiApp.Data;
using FacturiApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FacturiApp.Controllers;

[ApiController]
[Authorize]
[Route("[controller]/[action]")]
public class PlataController : ControllerBase
{
    private readonly AppDbContext _db;

    public PlataController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<List<PlataEntity>>> GetPlati()
    {
        var lista = await _db.Plati.ToListAsync();
        return Ok(lista);
    }

    [HttpGet]
    public async Task<ActionResult<PlataEntity>> GetPlataById(int id)
    {
        var plata = await _db.Plati
            .Include(p => p.Factura)
            .FirstOrDefaultAsync(p => p.IdPlata == id);

        if (plata == null)
            return NotFound($"Plata cu id-ul {id} nu a fost gasita.");
        return Ok(plata);
    }

    [HttpGet]
    public async Task<ActionResult<List<PlataEntity>>> GetPlatiByFactura(int nrFactura)
    {
        var plati = await _db.Plati
            .Where(p => p.NrFactura == nrFactura)
            .ToListAsync();

        if (plati.Count == 0)
            return NotFound($"Nu s-au gasit plati pentru factura cu numarul {nrFactura}.");
        return Ok(plati);
    }

    [HttpPost]
    public async Task<ActionResult> PostPlata([FromBody] PlataEntity plata)
    {
        var factura = await _db.Facturi.FindAsync(plata.NrFactura);
        if (factura == null)
            return BadRequest($"Factura cu numarul {plata.NrFactura} nu exista.");

        plata.Factura = null;
        _db.Plati.Add(plata);
        await _db.SaveChangesAsync();
        return Ok("Plata inregistrata cu succes!");
    }

    [HttpPut]
    public async Task<ActionResult> PutPlata([FromQuery] int id, [FromBody] PlataEntity plata)
    {
        var existing = await _db.Plati.FindAsync(id);
        if (existing == null)
            return NotFound($"Plata cu id-ul {id} nu a fost gasita.");

        existing.NrFactura = plata.NrFactura;
        existing.Suma = plata.Suma;
        existing.DataPlata = plata.DataPlata;
        existing.MetodaPlata = plata.MetodaPlata;
        existing.Status = plata.Status;

        await _db.SaveChangesAsync();
        return Ok("Plata actualizata cu succes!");
    }

    [HttpDelete]
    public async Task<ActionResult> DeletePlata(int id)
    {
        var plata = await _db.Plati.FindAsync(id);
        if (plata == null)
            return NotFound($"Plata cu id-ul {id} nu a fost gasita.");

        _db.Plati.Remove(plata);
        await _db.SaveChangesAsync();
        return Ok("Plata stearsa cu succes!");
    }
}
