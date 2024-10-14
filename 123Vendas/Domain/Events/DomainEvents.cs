namespace _123Vendas.Domain.Events
{
    public abstract class DomainEvents
    {
        public Guid VendaId { get; protected set; }
        public DateTime DataOcorrencia { get; protected set; }

        protected DomainEvents(Guid vendaId)
        {
            VendaId = vendaId;
            DataOcorrencia = DateTime.UtcNow;
        }
    }

    public class CompraCriada : DomainEvents
    {
        public CompraCriada(Guid vendaId) : base(vendaId) { }
    }

    public class CompraAlterada : DomainEvents
    {
        public CompraAlterada(Guid vendaId) : base(vendaId) { }
    }

    public class CompraCancelada : DomainEvents
    {
        public CompraCancelada(Guid vendaId) : base(vendaId) { }
    }

    public class ItemCancelado : DomainEvents
    {
        public ItemCancelado(Guid vendaId) : base(vendaId) { }
    }

    public static class DomainEventsManager
    {
        public static void PublicarEvento(DomainEvents evento)
        {
            Console.WriteLine($"Evento: {evento.GetType().Name}, VendaId: {evento.VendaId}, Data: {evento.DataOcorrencia}");
        }
    }
}
