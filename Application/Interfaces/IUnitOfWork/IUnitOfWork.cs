namespace Application.Interfaces.IUnitOfWork
{
    public interface IUnitOfWork
    {
        void BeginTransaction();
        void Commit();
        void Rollback();
        void SaveChanges();
        void Dispose();
    }
}
