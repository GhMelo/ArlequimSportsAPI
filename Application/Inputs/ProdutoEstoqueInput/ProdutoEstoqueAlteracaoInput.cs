using Application.Inputs.ProdutoInput;

namespace Application.Inputs.ProdutoEstoqueInput
{
    public class ProdutoEstoqueAlteracaoInput
    {
        public int ProdutoEstoqueId { get; set; }
        public string NotaFiscal { get; set; } = null!;
        public DateTime DataEntrada { get; set; }
        public ProdutoQuantidadeInput[] Produtos { get; set; } = null!;
    }
}
