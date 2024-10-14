namespace _123Vendas.Application.DTOs
{
    public class ItemVendaDTO
    {
        public Guid ProdutoId { get; set; }
        public string DescricaoProduto { get; set; }
        public int Quantidade { get; set; }
        public decimal ValorUnitario { get; set; }
        public decimal Desconto { get; set; }

        public ItemVendaDTO(Guid produtoId, string descricaoProduto, int quantidade, decimal valorUnitario, decimal desconto)
        {
            ProdutoId = produtoId;
            DescricaoProduto = descricaoProduto ?? throw new ArgumentNullException(nameof(descricaoProduto));
            Quantidade = quantidade;
            ValorUnitario = valorUnitario;
            Desconto = desconto;
        }
    }
}
