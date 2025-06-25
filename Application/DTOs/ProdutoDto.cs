namespace Application.DTOs
{
    public class ProdutoDto
    {
        public string Nome { get; set; } = null!;
        public string Descricao { get; set; } = null!;
        public double Preco { get; set; }
        public EsporteModalidadeDto EsporteModalidade { get; set; } = null!;
    }
}
