using _123Vendas.API.Controllers;
using _123Vendas.Application.DTOs;
using _123Vendas.Domain.Entities;
using _123Vendas.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Microsoft.Extensions.Logging;

namespace _123Vendas.Tests
{
    public class VendasControllerTests
    {
        private readonly Mock<IVendaRepository> _vendaRepositoryMock;
        private readonly Mock<ILogger<VendasController>> _loggerMock;
        private readonly VendasController _controller;

        public VendasControllerTests()
        {
            _vendaRepositoryMock = new Mock<IVendaRepository>();
            _loggerMock = new Mock<ILogger<VendasController>>();
            _controller = new VendasController(_vendaRepositoryMock.Object, _loggerMock.Object);
        }

        [Fact]
        public void CriarVenda_ShouldReturnBadRequest_WhenVendaDTOIsNull()
        {
            var result = _controller.CriarVenda(null);

            var badRequestResult = Xunit.Assert.IsType<BadRequestObjectResult>(result); 
            Xunit.Assert.Equal("Venda não pode ser nulo.", badRequestResult.Value); 
        }

        [Fact]
        public void CriarVenda_ShouldReturnBadRequest_WhenNomeClienteIsEmpty()
        {
            var vendaDTO = new CriarVendaDTO
            {
                NomeCliente = "",
                Filial = "Filial A",
                ClienteId = Guid.NewGuid(),
                Itens = new List<ItemVendaDTO>()
            };

            var result = _controller.CriarVenda(vendaDTO);

            var badRequestResult = Xunit.Assert.IsType<BadRequestObjectResult>(result);
            Xunit.Assert.Equal("Nome do cliente deve estar preenchido.", badRequestResult.Value);
        }

        [Fact]
        public void CriarVenda_ShouldReturnCreated_WhenVendaDTOIsValid()
        {
            var vendaDTO = new CriarVendaDTO
            {
                NomeCliente = "Cliente Exemplo",
                Filial = "Filial 1",
                ClienteId = Guid.NewGuid(),
                Itens = new List<ItemVendaDTO>
                {
                    new ItemVendaDTO(
                        Guid.NewGuid(), 
                        "Produto Exemplo",
                        2,
                        100.00m,
                        10.00m
                    )
                }
            };

            var result = _controller.CriarVenda(vendaDTO);

            var createdResult = Xunit.Assert.IsType<CreatedAtActionResult>(result);
            Xunit.Assert.NotNull(createdResult);
        }


        [Fact]
        public async Task ObterVenda_ShouldReturnNotFound_WhenVendaDoesNotExist()
        {
            var id = Guid.NewGuid();
            _vendaRepositoryMock.Setup(repo => repo.ObterPorIdAsync(id)).ReturnsAsync((Venda)null);

            var result = await _controller.ObterVenda(id);

            Xunit.Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task ObterVenda_ShouldReturnOk_WhenVendaExists()
        {
            var id = Guid.NewGuid();
            var venda = new Venda(1, new Cliente(Guid.NewGuid(), "Cliente Teste"), "Filial A");
            _vendaRepositoryMock.Setup(repo => repo.ObterPorIdAsync(id)).ReturnsAsync(venda);

            var result = await _controller.ObterVenda(id);

            var okResult = Xunit.Assert.IsType<OkObjectResult>(result);
            Xunit.Assert.Equal(venda, okResult.Value);
        }

        [Fact]
        public async Task AtualizarVenda_ShouldReturnNotFound_WhenVendaDoesNotExist()
        {
            var id = Guid.NewGuid();
            var vendaDTO = new CriarVendaDTO
            {
                NomeCliente = "Cliente Teste",
                Filial = "Filial A",
                ClienteId = Guid.NewGuid(),
                Itens = new List<ItemVendaDTO>()
            };
            _vendaRepositoryMock.Setup(repo => repo.ObterPorIdAsync(id)).ReturnsAsync((Venda)null);

            var result = await _controller.AtualizarVenda(id, vendaDTO);

            Xunit.Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task CancelarVenda_ShouldReturnNotFound_WhenVendaDoesNotExist()
        {
            var id = Guid.NewGuid();
            _vendaRepositoryMock.Setup(repo => repo.ObterPorIdAsync(id)).ReturnsAsync((Venda)null);

            var result = await _controller.CancelarVenda(id);

            Xunit.Assert.IsType<NotFoundResult>(result);
        }
    }
}
