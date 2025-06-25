using Domain.Entity;

namespace Domain.Interfaces.IRepository
{
    public interface IPedidoProdutoRepository : IRepository<PedidoProduto>
    {
        IEnumerable<PedidoProduto> obterTodosPorPedidoId(int id);
    }
}
