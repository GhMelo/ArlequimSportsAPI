namespace Application.DTOs
{
    public class PedidoDto
    {
        public DateTime DataPedido { get; set; }
        public string DocumentoCliente { get; set; }
        public string EmailCliente { get; set; }
        public UsuarioDto Vendedor { get; set; } = null!;
        public StatusPedidoDto StatusPedido { get; set; } = null!;
        public ICollection<PedidoProdutoDto> produtoPedido { get; set; } = null!;
    }
}
