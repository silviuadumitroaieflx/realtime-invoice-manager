using Microsoft.EntityFrameworkCore;
using ProdusService.Models;

namespace ProdusService.Data;

public class ProdusDbContext : DbContext
{
    public ProdusDbContext(DbContextOptions<ProdusDbContext> options) : base(options)
    { }

    public DbSet<ProdusEntity> Produse { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProdusEntity>(e =>
        {
            e.HasKey(p => p.IdProdus);
            e.Property(p => p.IdProdus).ValueGeneratedOnAdd();
            e.Property(p => p.PretUnitar).HasColumnType("decimal(18,2)");
        });
    }
}
