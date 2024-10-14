using _123Vendas.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace _123Vendas.Infrastructure.Repositories
{
    public interface IVendaRepository
    {
        Task<Venda> ObterPorIdAsync(Guid id);
        Task AdicionarAsync(Venda venda);
        Task AtualizarAsync(Venda venda);
        Task<bool> VendaExisteAsync(Guid id);
    }
}
