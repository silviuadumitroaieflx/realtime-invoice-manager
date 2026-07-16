using FacturiApp.Models;
using Microsoft.EntityFrameworkCore;

namespace FacturiApp.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
   { }
    public DbSet<ClientEntity> Clienti { get; set; }
    public DbSet<ProdusEntity> Produse { get; set; }
    public DbSet<FacturaEntity> Facturi { get; set; }
    public DbSet<ProdusFacturaEntity> ProdusFactura { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<PlataEntity> Plati { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // client
        modelBuilder.Entity<ClientEntity>(e =>
        {
            e.HasKey(c => c.IdClient);
            e.Property(c => c.IdClient).ValueGeneratedOnAdd();
        });

        //produs
        modelBuilder.Entity<ProdusEntity>(e =>
        {
            e.HasKey(p => p.IdProdus);
            e.Property(p => p.IdProdus).ValueGeneratedOnAdd();
            e.Property(p => p.PretUnitar).HasColumnType("decimal(18,2)");
        });

        //factura 
        modelBuilder.Entity<FacturaEntity>(e =>
        {
            e.HasKey(f => f.NrFactura);
            e.HasOne(f => f.Client)
             .WithMany()
             .HasForeignKey(f => f.IdClient)
             .OnDelete(DeleteBehavior.Restrict);
        });

        //produs factura
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

        //user
        modelBuilder.Entity<User>(e =>
        {
            e.HasKey(u => u.IdUser);
            e.Property(u => u.IdUser).ValueGeneratedOnAdd();
        });

        //plata
        modelBuilder.Entity<PlataEntity>(e =>
        {
            e.HasKey(p => p.IdPlata);
            e.Property(p => p.IdPlata).ValueGeneratedOnAdd();
            e.Property(p => p.Suma).HasColumnType("decimal(18,2)");
            e.HasOne(p => p.Factura)
             .WithMany()
             .HasForeignKey(p => p.NrFactura)
             .OnDelete(DeleteBehavior.Restrict);
        });

    }
}