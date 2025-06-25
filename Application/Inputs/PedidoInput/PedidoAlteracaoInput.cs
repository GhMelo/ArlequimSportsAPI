using Application.Inputs.ProdutoInput;

namespace Application.Inputs.PedidoInput
{
    public class PedidoAlteracaoInput
    {
        public string DocumentoCliente { get; set; }
        public string EmailCliente { get; set; }
        public int VendedorId { get; set; }
        public int StatusPedidoId { get; set; }
        public ProdutoQuantidadeInput[] Produtos { get; set; } = null!;
    }
}
