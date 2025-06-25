namespace Application.DTOs
{
    public class ProdutoEstoqueDto
    {
        public string NotaFiscal { get; set; } = null!;
        public int Quantidade { get; set; }
        public DateTime DataEntrada { get; set; }
        public ProdutoDto Produto { get; set; } = null!;
    }
}
