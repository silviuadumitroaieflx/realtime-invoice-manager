using FacturaService.Models;
using Microsoft.EntityFrameworkCore;

namespace FacturaService.Data;

public class FacturaDbContext : DbContext
{
    public FacturaDbContext(DbContextOptions<FacturaDbContext> options) : base(options)
    { }

    public DbSet<FacturaEntity> Facturi { get; set; }
    public DbSet<ProdusFacturaEntity> ProdusFactura { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // factura (fara relatia catre Client - clientul e in alt serviciu)
        modelBuilder.Entity<FacturaEntity>(e =>
        {
            e.HasKey(f => f.NrFactura);
        });

        // produs factura (liniile raman in aceeasi baza ca factura)
        modelBuilder.Entity<ProdusFacturaEntity>(e =>
        {
            e.HasKey(pf => pf.Id);
            e.Property(pf => pf.Id).ValueGeneratedOnAdd();
            e.Property(pf => pf.PretUnitar).HasColumnType("decimal(18,2)");
            e.HasOne(pf => pf.Factura)
             .WithMany(f => f.ProduseFactura)
             .HasForeignKey(pf => pf.NrFactura)
             .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
