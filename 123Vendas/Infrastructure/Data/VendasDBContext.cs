using Microsoft.EntityFrameworkCore;
using _123Vendas.Domain.Entities;

public class VendasDbContext : DbContext
{
    public VendasDbContext(DbContextOptions<VendasDbContext> options) : base(options)
    {
    }

    public DbSet<Venda> Vendas { get; set; } 
    public DbSet<Cliente> Clientes { get; set; } 

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Venda>()
            .HasKey(v => v.Id);

        modelBuilder.Entity<Cliente>()
            .HasKey(c => c.Id);
    }
}
