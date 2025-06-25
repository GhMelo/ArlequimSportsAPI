using Application.Inputs.ProdutoInput;

namespace Application.Inputs.PedidoInput
{
    public class PedidoCadastroInput
    {
        public string DocumentoCliente { get; set; }
        public string EmailCliente { get; set; }
        public ProdutoQuantidadeInput[] Produtos { get; set; } = null!;
    }
}
