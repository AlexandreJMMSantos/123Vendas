using _123Vendas.Application.DTOs;
using _123Vendas.Domain.Entities;
using _123Vendas.Domain.Events;
using _123Vendas.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace _123Vendas.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VendasController : ControllerBase
    {
        private readonly IVendaRepository _vendaRepository;
        private readonly ILogger<VendasController> _logger;

        public VendasController(IVendaRepository vendaRepository, ILogger<VendasController> logger)
        {
            _vendaRepository = vendaRepository;
            _logger = logger;
        }

        [HttpPost]
        public IActionResult CriarVenda([FromBody] CriarVendaDTO vendaDTO)
        {
            _logger.LogInformation("Tentativa de criação de venda.");

            if (vendaDTO == null)
            {
                _logger.LogWarning("VendaDTO nulo recebido.");
                return BadRequest("Venda não pode ser nulo.");
            }

            if (string.IsNullOrWhiteSpace(vendaDTO.NomeCliente))
            {
                _logger.LogWarning("Nome do cliente vazio ou nulo.");
                return BadRequest("Nome do cliente deve estar preenchido.");
            }

            if (string.IsNullOrWhiteSpace(vendaDTO.Filial))
            {
                _logger.LogWarning("Filial vazia ou nula.");
                return BadRequest("Filial deve estar preenchido.");
            }

            if (vendaDTO.ClienteId == Guid.Empty)
            {
                _logger.LogWarning("ID do cliente vazio.");
                return BadRequest("ID Cliente deve estar preenchido.");
            }

            var cliente = new Cliente(vendaDTO.ClienteId, vendaDTO.NomeCliente);
            var venda = new Venda(vendaDTO.NumeroVenda, cliente, vendaDTO.Filial);

            foreach (var itemDTO in vendaDTO.Itens)
            {
                if (itemDTO == null)
                {
                    _logger.LogWarning("Item nulo encontrado.");
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

            _vendaRepository.AdicionarAsync(venda);
            _logger.LogInformation("Venda criada com sucesso, ID: {VendaId}", venda.Id);

            DomainEventsManager.PublicarEvento(new CompraCriada(venda.Id));

            return CreatedAtAction(nameof(ObterVenda), new { id = venda.Id }, venda);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterVenda(Guid id)
        {
            _logger.LogInformation("Tentativa de obtenção de venda, ID: {VendaId}", id);

            var venda = _vendaRepository.ObterPorIdAsync(id);
            if (venda == null)
            {
                _logger.LogWarning("Venda não encontrada, ID: {VendaId}", id);
                return NotFound();
            }

            _logger.LogInformation("Venda encontrada, ID: {VendaId}", id);
            return Ok(venda);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarVenda(Guid id, [FromBody] CriarVendaDTO vendaDTO)
        {
            _logger.LogInformation("Tentativa de atualização de venda, ID: {VendaId}", id);

            if (vendaDTO == null)
            {
                _logger.LogWarning("Dados da venda nulos.");
                return BadRequest("Dados da venda estão nulos.");
            }

            if (string.IsNullOrWhiteSpace(vendaDTO.NomeCliente))
            {
                _logger.LogWarning("Nome cliente nulo ou vazio.");
                return BadRequest("Nome cliente deve estar preenchido.");
            }

            if (vendaDTO.ClienteId == Guid.Empty)
            {
                _logger.LogWarning("ID do cliente vazio.");
                return BadRequest("ID Cliente deve estar preenchido.");
            }

            var venda = await _vendaRepository.ObterPorIdAsync(id);
            if (venda == null)
            {
                _logger.LogWarning("Venda não encontrada, ID: {VendaId}", id);
                return NotFound();
            }

            venda.NumeroVenda = vendaDTO.NumeroVenda;
            venda.Cliente = new Cliente(vendaDTO.ClienteId, vendaDTO.NomeCliente);
            venda.Filial = vendaDTO.Filial;

            venda.Itens.Clear();
            foreach (var itemDTO in vendaDTO.Itens)
            {
                if (itemDTO == null)
                {
                    _logger.LogWarning("Item da venda está nulo.");
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
            _logger.LogInformation("Venda atualizada com sucesso, ID: {VendaId}", id);

            DomainEventsManager.PublicarEvento(new CompraAlterada(venda.Id));

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> CancelarVenda(Guid id)
        {
            _logger.LogInformation("Tentativa de cancelamento de venda, ID: {VendaId}", id);

            var venda = await _vendaRepository.ObterPorIdAsync(id);
            if (venda == null)
            {
                _logger.LogWarning("Venda não encontrada, ID: {VendaId}", id);
                return NotFound();
            }

            venda.CancelarVenda();
            await _vendaRepository.AtualizarAsync(venda);
            _logger.LogInformation("Venda cancelada com sucesso, ID: {VendaId}", id);

            DomainEventsManager.PublicarEvento(new CompraCancelada(venda.Id));

            return NoContent();
        }
    }
}
