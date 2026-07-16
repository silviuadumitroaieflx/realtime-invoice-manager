using FacturiApp.Data;
using FacturiApp.Hubs;
using FacturiApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace FacturiApp.Controllers;

[ApiController]
[Authorize]
[Route("[controller]/[action]")]
public class ClientController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IHubContext<ClientHub> _hub;

    public ClientController(AppDbContext db, IHubContext<ClientHub> hub)
    {
        _db = db;
        _hub = hub;
    }
    [HttpGet]
    public async Task<ActionResult<List<ClientEntity>>> GetClienti()
    {
        var lista = await _db.Clienti.ToListAsync();
        return Ok(lista);
    }
    [HttpGet]
    public async Task<ActionResult<ClientEntity>> GetClientById(int id)
    {
        var client = await _db.Clienti.FindAsync(id);
        if (client == null)
            return NotFound($"Clientul cu id-ul {id} nu a fost gasit.");
        return Ok(client);
    }
    [HttpGet]
    public async Task<ActionResult<List<ClientEntity>>> GetClientiByNume(string nume)
    {
        var clienti = await _db.Clienti
            .Where(c => c.NumeClient == nume)
            .ToListAsync();

        if (clienti.Count == 0)
            return NotFound($"Nu s-au gasit clienti cu numele {nume}.");
        return Ok(clienti);
    }
    [HttpPost]
    public async Task<ActionResult> PostClient([FromBody] ClientEntity client)
    {
        _db.Clienti.Add(client);
        await _db.SaveChangesAsync();
        await _hub.Clients.All.SendAsync("ClientiModificati", "adaugat: " + client.NumeClient);
        return Ok("Client adaugat cu succes!");
    }

    [HttpPut]
    public async Task<ActionResult> PutClient([FromQuery] int id, [FromBody] ClientEntity client)
    {
        var existing = await _db.Clienti.FindAsync(id);
        if (existing == null)
            return NotFound($"Clientul cu id-ul {id} nu a fost gasit.");

        existing.NumeClient = client.NumeClient;
        existing.Email = client.Email;
        existing.Telefon = client.Telefon;
        existing.Adresa = client.Adresa;

        await _db.SaveChangesAsync();
        await _hub.Clients.All.SendAsync("ClientiModificati", "modificat: " + existing.NumeClient);
        return Ok("Client actualizat cu succes!");
    }
    [HttpDelete]
    public async Task<ActionResult> DeleteClient(int id)
    {
        var client = await _db.Clienti.FindAsync(id);
        if (client == null)
            return NotFound($"Clientul cu id-ul {id} nu a fost gasit.");

        _db.Clienti.Remove(client);
        await _db.SaveChangesAsync();
        await _hub.Clients.All.SendAsync("ClientiModificati", "sters: " + client.NumeClient);
        return Ok("Client sters cu succes!");
    }

}
