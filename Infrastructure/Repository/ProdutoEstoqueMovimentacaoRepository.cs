using Domain.Entity;
using Domain.Interfaces.IRepository;

namespace Infrastructure.Repository
{
    public class ProdutoEstoqueMovimentacaoRepository : EFRepository<ProdutoEstoqueMovimentacao>, IProdutoEstoqueMovimentacaoRepository
    {
        public ProdutoEstoqueMovimentacaoRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
