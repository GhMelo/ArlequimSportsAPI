using Domain.Entity;
using Domain.Interfaces.IRepository;

namespace Infrastructure.Repository
{
    public class TipoOperacaoRepository : EFRepository<TipoOperacao>, ITipoOperacaoRepository
    {
        public TipoOperacaoRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
