using Domain.Interfaces.IRepository;

namespace Infrastructure.Repository
{
    public class ProdutoRepository : EFRepository<Produto>, IProdutoRepository
    {
        public ProdutoRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
