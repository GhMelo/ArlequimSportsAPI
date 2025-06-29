namespace Application.Interfaces.IUnitOfWork
{
    public interface IUnitOfWork
    {
        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
        Task DisposeAsync();
        Task SaveChangesAsync();
    }
}
