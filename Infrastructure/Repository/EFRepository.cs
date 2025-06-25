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
            try
            {
                _dbSet.Update(entidade);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public void Cadastrar(T entidade)
        {
            try
            {
                _dbSet.Add(entidade);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public void Deletar(int id)
        {
            try
            {
                _dbSet.Remove(ObterPorId(id));
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public T ObterPorId(int id)
        {
            try
            {
                return _dbSet.FirstOrDefault(entity => entity.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public IList<T> ObterTodos()
        {
            try
            {
                return _dbSet.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
    }
}
