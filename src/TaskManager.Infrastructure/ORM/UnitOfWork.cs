using TaskManager.Domain.Interfaces;
using TaskManager.Domain.ORM;
using TaskManager.Infrastructure.Repositories;

namespace TaskManager.Infrastructure.ORM;

public class UnitOfWork(TaskManagerDbContext context) : IUnitOfWork
{
    private IUserRepository? _userRepository;
    public IUserRepository UserRepository => _userRepository ??= new UserRepository(context);
    
    private IProjectRepository? _projectRepository;
    public IProjectRepository ProjectRepository => _projectRepository ??= new ProjectRepository(context);
    
    private ITaskRepository? _taskRepository;
    public ITaskRepository TaskRepository => _taskRepository ??= new TaskRepository(context);
    
    private ITaskCommentRepository? _taskCommentRepository;
    public ITaskCommentRepository TaskCommentRepository => _taskCommentRepository ??= new TaskCommentRepository(context);
    
    private ITaskHistoryRepository? _taskHistoryRepository;
    public ITaskHistoryRepository TaskHistoryRepository => _taskHistoryRepository ??= new TaskHistoryRepository(context);
    
    public async Task SaveAsync()
    {
        await context.SaveChangesAsync();
    }
    
    public void BeginTransaction()
    {
        context.Database.BeginTransaction();
    }
    
    public async Task BeginTransactionAsync()
    {
        await context.Database.BeginTransactionAsync();
    }

    public async Task CommitAsync()
    {
        await context.Database.CommitTransactionAsync();
    }
    
    public async Task RollbackAsync()
    {
        await context.Database.RollbackTransactionAsync();
    }

    public void Dispose()
    {
        context.Dispose();
        GC.SuppressFinalize(this);
    }

    public async ValueTask DisposeAsync()
    {
        await context.DisposeAsync();
        GC.SuppressFinalize(this);
    }
}