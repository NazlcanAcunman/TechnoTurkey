namespace EventTicket.Core.Interfaces;

public interface IUnitOfWork : IDisposable, IAsyncDisposable  
{
    IRepository<T> Repository<T>() where T : class;
    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
}
