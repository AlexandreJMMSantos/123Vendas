using _123Vendas.Domain.Events;

namespace _123Vendas.Domain.Entities
{
    public class Venda
    {
        public Venda()
        {
                
        }

        public Guid Id { get; private set; }
        public int NumeroVenda { get; set; }
        public Guid ClienteId { get; set; }
        public string Filial { get; set; }
        public List<ItemVenda> Itens { get; private set; } = new List<ItemVenda>(); 
        public decimal ValorTotal { get; private set; }

        public Venda(int numeroVenda, Guid clienteId , string filial)
        {
            Id = Guid.NewGuid(); 
            NumeroVenda = numeroVenda;
            ClienteId = clienteId;
            Filial = filial;
        }

        public void AdicionarItem(ItemVenda item)
        {
            Itens.Add(item); 
            ValorTotal += item.ValorUnitario * item.Quantidade - item.Desconto; 
        }

        public void CancelarVenda()
        {
            
        }
    }

}
