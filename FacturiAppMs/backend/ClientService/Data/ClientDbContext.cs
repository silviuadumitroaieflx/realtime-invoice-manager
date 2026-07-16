using ClientService.Models;
using Microsoft.EntityFrameworkCore;

namespace ClientService.Data;

public class ClientDbContext : DbContext
{
    public ClientDbContext(DbContextOptions<ClientDbContext> options) : base(options)
    { }

    public DbSet<ClientEntity> Clienti { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ClientEntity>(e =>
        {
            e.HasKey(c => c.IdClient);
            e.Property(c => c.IdClient).ValueGeneratedOnAdd();
        });
    }
}
