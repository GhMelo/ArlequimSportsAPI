using Domain.Entity;
using Domain.Interfaces.IRepository;

namespace Infrastructure.Repository
{
    public class PedidoProdutoRepository : EFRepository<PedidoProduto>, IPedidoProdutoRepository
    {
        public PedidoProdutoRepository(ApplicationDbContext context) : base(context)
        {

        }

        public IEnumerable<PedidoProduto> obterTodosPorPedidoId(int id)
            => _dbSet.Where(entity => entity.Id == id);
    }
}
