using _123Vendas.Domain.Entities;
using Microsoft.EntityFrameworkCore;

public class DatabaseInitializer
{
    private readonly VendasDbContext _context;

    public DatabaseInitializer(VendasDbContext context)
    {
        _context = context;
    }

    public async Task InitializeAsync()
    {
        await _context.Database.EnsureCreatedAsync();

        if (await _context.Clientes.AnyAsync())
        {
            return;
        }

        _context.Clientes.Add(new Cliente
        {
            Id = new Guid("122C2AFC-9331-4F12-8F08-D0A742E1EF33"),
            Nome = "Alexandre Santos"
        });

        await _context.SaveChangesAsync();
    }
}
