using Microsoft.EntityFrameworkCore;
using _123Vendas.Domain.Entities;
using _123Vendas.Infrastructure.Repositories;
using Castle.Components.DictionaryAdapter.Xml;

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
        catch (Exception)
        {
            throw;
        }
    }

    public void AtualizarAsync(Venda venda)
    {
        try
        {
            _context.Vendas.Attach(venda);
            _context.Vendas.Update(venda);
            _context.SaveChanges();
        }
        catch (DbUpdateConcurrencyException)
        {
            throw;
        }
        catch (DbUpdateException)
        {
            throw;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public void RemoveAsync(Guid id)
    {
        try
        {
            var venda = _context.Vendas.Find(id);

            _context.Vendas.Remove(venda);

            _context.SaveChangesAsync();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<bool> VendaExisteAsync(Guid id)
    {
        return await _context.Vendas.AnyAsync(v => v.Id == id);
    }
}


