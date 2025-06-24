namespace Domain.Entity
{
    public class Pedido : EntityBase
    {
        public DateTime DataPedido { get; set; }
        public int VendedorId { get; set; }
        public string DocumentoCliente { get; set; }
        public string StatusPedidoId { get; set; }
        public virtual Usuario Vendedor { get; set; } = null!;
        public virtual StatusPedido StatusPedido { get; set; } = null!;
    }
}
