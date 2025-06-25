namespace Application.DTOs
{
    public class PedidoProdutoDto
    {
        public int PedidoId { get; set; }
        public int ProdutoId { get; set; }
        public int Quantidade { get; set; }
        public int ProdutoEstoqueId { get; set; }
    }
}
