using Domain.Interfaces;

namespace Domain.ORM;

public interface IUnitOfWork : IDisposable, IAsyncDisposable
{
    IUserRepository UserRepository { get; }
    IProjectRepository ProjectRepository { get; }
    ITaskRepository TaskRepository { get; }
    void BeginTransaction();
    Task BeginTransactionAsync();
    void Commit();
    Task CommitAsync();
    void Rollback();
    Task RollbackAsync();
}