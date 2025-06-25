using Domain.Entity;
using Domain.Interfaces.IRepository;

namespace Infrastructure.Repository
{
    public class ProdutoEstoqueRepository : EFRepository<ProdutoEstoque>, IProdutoEstoqueRepository
    {
        public ProdutoEstoqueRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
