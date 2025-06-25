using Application.Inputs.ProdutoInput;

namespace Application.Inputs.ProdutoEstoqueInput
{
    public class ProdutoEstoqueCadastroInput
    {
        public string NotaFiscal { get; set; } = null!;
        public ProdutoQuantidadeInput[] Produtos { get; set; } = null!;
    }
}
