using TaskManager.Domain.Interfaces;

namespace TaskManager.Domain.ORM;

public interface IUnitOfWork : IDisposable, IAsyncDisposable
{
    IUserRepository UserRepository { get; }
    IProjectRepository ProjectRepository { get; }
    ITaskRepository TaskRepository { get; }
    ITaskCommentRepository TaskCommentRepository { get; }
    ITaskHistoryRepository TaskHistoryRepository { get; }
    void BeginTransaction();
    Task BeginTransactionAsync();
    void Commit();
    Task CommitAsync();
    void Rollback();
    Task RollbackAsync();
}