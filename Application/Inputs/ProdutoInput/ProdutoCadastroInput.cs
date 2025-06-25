namespace Application.Inputs.ProdutoInput
{
    public class ProdutoCadastroInput
    {
        public string Nome { get; set; } = null!;
        public string Descricao { get; set; } = null!;
        public double Preco { get; set; }
        public int EsporteModalidadeId { get; set; }
    }
}
