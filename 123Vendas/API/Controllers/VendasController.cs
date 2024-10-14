using _123Vendas.Application.DTOs;
using _123Vendas.Domain.Entities;
using _123Vendas.Domain.Events;
using _123Vendas.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace _123Vendas.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VendasController : ControllerBase
    {
        private readonly IVendaRepository _vendaRepository;

        public VendasController(IVendaRepository vendaRepository)
        {
            _vendaRepository = vendaRepository;
        }

        [HttpPost]
        public IActionResult CriarVenda([FromBody] CriarVendaDTO vendaDTO)
        {
            if (vendaDTO == null)
            {
                return BadRequest("Venda não pode ser nulo.");
            }

            if (string.IsNullOrWhiteSpace(vendaDTO.NomeCliente))
            {
                return BadRequest("Nome do cliente deve estar preenchido.");
            }

            if (string.IsNullOrWhiteSpace(vendaDTO.Filial))
            {
                return BadRequest("Filial deve estar preenchido.");
            }

            if (vendaDTO.ClienteId == Guid.Empty)
            {
                return BadRequest("ID Cliente deve estar preenchido.");
            }

            var cliente = new Cliente(vendaDTO.ClienteId, vendaDTO.NomeCliente);
            var venda = new Venda(vendaDTO.NumeroVenda, cliente, vendaDTO.Filial);

            foreach (var itemDTO in vendaDTO.Itens)
            {
                if (itemDTO == null)
                {
                    return BadRequest("Item nulo.");
                }

                var item = new ItemVenda(
                    itemDTO.ProdutoId,
                    itemDTO.DescricaoProduto,
                    itemDTO.Quantidade,
                    itemDTO.ValorUnitario,
                    itemDTO.Desconto
                );

                venda.AdicionarItem(item); 
            }

            foreach (var itemDTO in vendaDTO.Itens)
            {
                var item = new ItemVenda(
                    itemDTO.ProdutoId,
                    itemDTO.DescricaoProduto,
                    itemDTO.Quantidade,
                    itemDTO.ValorUnitario,
                    itemDTO.Desconto
                );
                venda.AdicionarItem(item);
            }

            _vendaRepository.AdicionarAsync(venda);

            DomainEventsManager.PublicarEvento(new CompraCriada(venda.Id));

            return CreatedAtAction(nameof(ObterVenda), new { id = venda.Id }, venda);
        }




        [HttpGet("{id}")]
        public IActionResult ObterVenda(Guid id)
        {
            var venda = _vendaRepository.ObterPorIdAsync(id);
            if (venda == null) return NotFound();
            return Ok(venda);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarVenda(Guid id, [FromBody] CriarVendaDTO vendaDTO)
        {
            if (vendaDTO == null)
            {
                return BadRequest("Dados da venda estão nulos.");
            }

            if (string.IsNullOrWhiteSpace(vendaDTO.NomeCliente))
            {
                return BadRequest("Nome cliente deve estar preenchido.");
            }

            if (vendaDTO.ClienteId == Guid.Empty)
            {
                return BadRequest("ID Cliente deve estar preenchido.");
            }

            if (string.IsNullOrEmpty(vendaDTO.Filial))
            {
                return BadRequest("Filial deve estar preenchido.");
            }

            var venda = await _vendaRepository.ObterPorIdAsync(id); 
            if (venda == null) return NotFound();

            venda.NumeroVenda = vendaDTO.NumeroVenda;
            venda.Cliente = new Cliente(vendaDTO.ClienteId, vendaDTO.NomeCliente);
            venda.Filial = vendaDTO.Filial;

            venda.Itens.Clear();
            foreach (var itemDTO in vendaDTO.Itens)
            {
                if (itemDTO == null)
                {
                    return BadRequest("Item da venda está nulo.");
                }

                var item = new ItemVenda(
                    itemDTO.ProdutoId,
                    itemDTO.DescricaoProduto,
                    itemDTO.Quantidade,
                    itemDTO.ValorUnitario,
                    itemDTO.Desconto
                );

                venda.AdicionarItem(item);
            }

            await _vendaRepository.AtualizarAsync(venda); 

            DomainEventsManager.PublicarEvento(new CompraAlterada(venda.Id));

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> CancelarVenda(Guid id)
        {
            var venda = await _vendaRepository.ObterPorIdAsync(id);
            if (venda == null) return NotFound();

            venda.CancelarVenda(); 
            await _vendaRepository.AtualizarAsync(venda); 

            DomainEventsManager.PublicarEvento(new CompraCancelada(venda.Id));

            return NoContent();
        }

    }

}
