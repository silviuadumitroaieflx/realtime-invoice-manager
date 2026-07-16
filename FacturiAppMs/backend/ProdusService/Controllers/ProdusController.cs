using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProdusService.Data;
using ProdusService.Models;

namespace ProdusService.Controllers;

[ApiController]
[Authorize]
[Route("[controller]/[action]")]
public class ProdusController : ControllerBase
{
    private readonly ProdusDbContext _db;

    public ProdusController(ProdusDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<List<ProdusEntity>>> GetProduse()
    {
        var lista = await _db.Produse.ToListAsync();
        return Ok(lista);
    }

    [HttpGet]
    public async Task<ActionResult<ProdusEntity>> GetProdusById(int id)
    {
        var produs = await _db.Produse.FindAsync(id);
        if (produs == null)
            return NotFound($"Produsul cu id-ul {id} nu a fost gasit.");
        return Ok(produs);
    }

    [HttpGet]
    public async Task<ActionResult<List<ProdusEntity>>> GetProduseByNume(string nume)
    {
        var produse = await _db.Produse
            .Where(p => p.NumeProdus == nume)
            .ToListAsync();

        if (produse.Count == 0)
            return NotFound($"Nu s-au gasit produse cu numele {nume}.");
        return Ok(produse);
    }

    [HttpPost]
    public async Task<ActionResult> PostProdus([FromBody] ProdusEntity produs)
    {
        _db.Produse.Add(produs);
        await _db.SaveChangesAsync();
        return Ok("Produs adaugat cu succes!");
    }

    [HttpPut]
    public async Task<ActionResult> PutProdus([FromQuery] int id, [FromBody] ProdusEntity produs)
    {
        var existing = await _db.Produse.FindAsync(id);
        if (existing == null)
            return NotFound($"Produsul cu id-ul {id} nu a fost gasit.");

        existing.NumeProdus = produs.NumeProdus;
        existing.PretUnitar = produs.PretUnitar;
        existing.UnitateMasura = produs.UnitateMasura;

        await _db.SaveChangesAsync();
        return Ok("Produs actualizat cu succes!");
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteProdus(int id)
    {
        var produs = await _db.Produse.FindAsync(id);
        if (produs == null)
            return NotFound($"Produsul cu id-ul {id} nu a fost gasit.");

        _db.Produse.Remove(produs);
        await _db.SaveChangesAsync();
        return Ok("Produs sters cu succes!");
    }
}
