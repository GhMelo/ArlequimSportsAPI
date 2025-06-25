using Domain.Entity;
using Domain.Interfaces.IRepository;

namespace Infrastructure.Repository
{
    public class EsporteModalidadeRepository : EFRepository<EsporteModalidade>, IEsporteModalidadeRepository
    {
        public EsporteModalidadeRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
