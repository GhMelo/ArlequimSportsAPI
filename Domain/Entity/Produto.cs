using Domain.Entity;

public class Produto : EntityBase
{
    public string Nome { get; set; } = null!;
    public string Descricao { get; set; } = null!;
    public double Preco { get; set; }
    public int EsporteModalidadeId { get; set; }
    public virtual EsporteModalidade EsporteModalidade  { get; set; }
    public virtual ICollection<ProdutoEstoque> ProdutoEstoque { get; set; }
    public virtual ICollection<PedidoProduto> PedidoProduto { get; set; }
}