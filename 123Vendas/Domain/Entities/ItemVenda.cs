namespace _123Vendas.Domain.Entities
{
    public class ItemVenda
    {
        public Guid Id { get; set; }
        public Guid VendaId { get; set; }
        public Venda Venda { get; set; } // Propriedade de navegação
        public Guid ProdutoId { get; set; }
        public string ProdutoNome { get; set; }
        public decimal Quantidade { get; set; }
        public decimal ValorUnitario { get; set; }
        public decimal Desconto { get; set; }
        public decimal ValorTotal { get; set; }
        public bool Cancelado { get; set; }

        // Construtor sem parâmetros (necessário para o EF Core)
        public ItemVenda() { }

        // Construtor com parâmetros, sem a entidade Venda
        public ItemVenda(Guid produtoId, string produtoNome, decimal quantidade, decimal valorUnitario, decimal desconto)
        {
            Id = Guid.NewGuid();
            ProdutoId = produtoId;
            ProdutoNome = produtoNome;
            Quantidade = quantidade;
            ValorUnitario = valorUnitario;
            Desconto = desconto;
            ValorTotal = (quantidade * valorUnitario) - desconto;
        }
    }
}
