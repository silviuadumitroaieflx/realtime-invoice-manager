using AuthService.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Data;

public class AuthDbContext : DbContext
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
    { }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(e =>
        {
            e.HasKey(u => u.IdUser);
            e.Property(u => u.IdUser).ValueGeneratedOnAdd();
        });
    }
}
