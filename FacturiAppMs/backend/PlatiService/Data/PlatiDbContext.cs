using Microsoft.EntityFrameworkCore;
using PlatiService.Models;

namespace PlatiService.Data;

public class PlatiDbContext : DbContext
{
    public PlatiDbContext(DbContextOptions<PlatiDbContext> options) : base(options)
    { }

    public DbSet<PlataEntity> Plati { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PlataEntity>(e =>
        {
            e.HasKey(p => p.IdPlata);
            e.Property(p => p.IdPlata).ValueGeneratedOnAdd();
            e.Property(p => p.Suma).HasColumnType("decimal(18,2)");
        });
    }
}
