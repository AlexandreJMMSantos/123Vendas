using Microsoft.EntityFrameworkCore;
using _123Vendas.Domain.Entities;
using _123Vendas.Infrastructure.Repositories;

public class VendaRepository : IVendaRepository
{
    private readonly VendasDbContext _context;

    public VendaRepository(VendasDbContext context)
    {
        _context = context;
    }

    public async Task<Venda> ObterPorIdAsync(Guid id)
    {
        return await _context.Vendas.FirstOrDefaultAsync(v => v.Id == id);
    }

    public async Task AdicionarAsync(Venda venda)
    {
        try
        {
            await _context.Vendas.AddAsync(venda);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task AtualizarAsync(Venda venda) 
    {
        _context.Vendas.Update(venda);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> VendaExisteAsync(Guid id)
    {
        return await _context.Vendas.AnyAsync(v => v.Id == id);
    }
}


