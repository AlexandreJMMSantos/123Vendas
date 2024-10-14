using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using _123Vendas.Domain.Entities;

public interface IClienteRepository
{
    Task<Cliente> ObterPorIdAsync(Guid id);
    Task<List<Cliente>> ObterTodosAsync();
    Task AdicionarAsync(Cliente cliente);
    Task AtualizarAsync(Cliente cliente);
    Task RemoverAsync(Guid id);
}
