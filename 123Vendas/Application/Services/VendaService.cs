using _123Vendas.Domain.Entities;

public class VendaService
{
    private readonly VendasDbContext _context;

    public VendaService(VendasDbContext context)
    {
        _context = context;
    }

    public void CreateVenda(Venda venda)
    {
        _context.Vendas.Add(venda);
        _context.SaveChanges();
    }
}
