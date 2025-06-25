namespace Domain.Entity
{
    public class Pedido : EntityBase
    {
        public DateTime DataPedido { get; set; }
        public int VendedorId { get; set; }
        public string DocumentoCliente { get; set; }
        public string EmailCliente { get; set; }
        public int StatusPedidoId { get; set; }
        public virtual Usuario Vendedor { get; set; } = null!;
        public virtual StatusPedido StatusPedido { get; set; } = null!;
        public virtual ICollection<PedidoProduto> PedidoProduto { get; set; }
        public Pedido()
        {
            DataPedido = DateTime.Now;
        }
    }
}
