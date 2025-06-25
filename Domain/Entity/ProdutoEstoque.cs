namespace Domain.Entity
{
    public class ProdutoEstoque : EntityBase
    {
        public int ProdutoId { get; set; }
        public string NotaFiscal { get; set; } = null!;
        public int Quantidade { get; set; }
        public DateTime DataEntrada { get; set; }
        public virtual Produto Produto { get; set; } = null!;
        public ProdutoEstoque()
        {
            DataEntrada = DateTime.Now;
        }
    }
}
