namespace Application.Inputs.ProdutoInput
{
    public class ProdutoAlteracaoInput
    {
        public int ProdutoId { get; set; }
        public string Nome { get; set; } = null!;
        public string Descricao { get; set; } = null!;
        public double Preco { get; set; }
        public int EsporteModalidadeId { get; set; }
    }
}
