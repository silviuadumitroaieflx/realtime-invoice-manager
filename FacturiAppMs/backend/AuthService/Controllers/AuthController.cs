using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AuthService.Data;
using AuthService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace AuthService.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class AuthController : ControllerBase
{
    private readonly AuthDbContext _db;
    private readonly IConfiguration _config;

    public AuthController(AuthDbContext db, IConfiguration config)
    {
        _db = db;
        _config = config;
    }

    [HttpPost]
    public async Task<ActionResult> Register([FromBody] RegisterRequest req)
    {
        var exista = await _db.Users.AnyAsync(u => u.Username == req.Username);
        if (exista)
            return BadRequest("Username deja folosit.");

        var user = new User
        {
            Username = req.Username,
            Nume = req.Nume,
            Email = req.Email,
            PasswordHash = HashParola(req.Password),
            Role = "User"
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        return Ok("Cont creat cu succes!");
    }

    [HttpPut]
    public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordRequest req)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == req.Username);

        if (user == null || user.Email != req.Email)
            return BadRequest("Username sau email gresit.");

        user.PasswordHash = HashParola(req.NouaParola);
        await _db.SaveChangesAsync();
        return Ok("Parola a fost schimbata. Te poti autentifica.");
    }

    [HttpPost]
    public async Task<ActionResult> Login([FromBody] LoginRequest req)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == req.Username);

        if (user == null || user.PasswordHash != HashParola(req.Password))
            return Unauthorized("Username sau parola gresita.");

        var token = GenereazaToken(user);
        return Ok(new { token });
    }

    private static string HashParola(string parola)
    {
        using var sha = SHA256.Create();
        byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(parola));
        return Convert.ToBase64String(bytes);
    }

    private string GenereazaToken(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Username!),
            new Claim(ClaimTypes.Role, user.Role!)
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
