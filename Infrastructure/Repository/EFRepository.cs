using Domain.Entity;
using Domain.Interfaces.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class EFRepository<T> : IRepository<T> where T : EntityBase
    {
        protected ApplicationDbContext _context { get; set; }
        protected DbSet<T> _dbSet { get; set; }

        public EFRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public void Alterar(T entidade)
        {
                _dbSet.Update(entidade);
                _context.SaveChanges();
        }

        public void Cadastrar(T entidade)
        {
           
                _dbSet.Add(entidade);
                _context.SaveChanges();
        }

        public void Deletar(int id)
        {
                _dbSet.Remove(ObterPorId(id));
                _context.SaveChanges();
        }

        public T ObterPorId(int id)
        {
                return _dbSet.FirstOrDefault(entity => entity.Id == id);
        }

        public IList<T> ObterTodos()
        {
                return _dbSet.ToList();
        }
    }
}
