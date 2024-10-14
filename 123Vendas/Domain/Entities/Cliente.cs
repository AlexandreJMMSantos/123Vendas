namespace _123Vendas.Domain.Entities
{
    public class Cliente
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }

        public Cliente()
        {
                
        }

        public Cliente(Guid id, string nome)
        {
            Id = id;
            Nome = nome;
        }
    }
}
