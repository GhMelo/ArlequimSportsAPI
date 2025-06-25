namespace Domain.Entity
{
    public class ProdutoEstoqueMovimentacao : EntityBase
    {
        public int ProdutoEstoqueId { get; set; }
        public int TipoOperacaoId { get; set; }
        public int Quantidade { get; set; }
        public int IdUsuario { get; set; }
        public DateTime DataMovimentacao { get; set; }
        public virtual ProdutoEstoque ProdutoEstoque { get; set; } = null!;
        public virtual TipoOperacao TipoOperacao { get; set; } = null!;
        public virtual Usuario Usuario { get; set; } = null!;
        public ProdutoEstoqueMovimentacao()
        {
            DataMovimentacao = DateTime.Now;
        }
    }
}
