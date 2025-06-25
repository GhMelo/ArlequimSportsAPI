using Domain.Entity;
using Domain.Interfaces.IRepository;

namespace Infrastructure.Repository
{
    public class StatusPedidoRepository : EFRepository<StatusPedido>, IStatusPedidoRepository
    {
        public StatusPedidoRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
