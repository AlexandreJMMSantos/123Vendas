namespace _123Vendas.Application.DTOs
{
    public class CriarVendaDTO
    {
        public int NumeroVenda { get; set; }
        public Guid ClienteId { get; set; }
        public string? NomeCliente { get; set; }
        public string? Filial { get; set; }
        public List<ItemVendaDTO> Itens { get; set; } = new List<ItemVendaDTO>();
    }
}
