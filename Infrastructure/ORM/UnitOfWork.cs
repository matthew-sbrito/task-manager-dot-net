using Infrastructure.Repositories;
using Domain.Interfaces;
using Domain.ORM;

namespace Infrastructure.ORM;

public class UnitOfWork(TaskManagerDbContext context) : IUnitOfWork
{
    private IUserRepository? _userRepository;
    public IUserRepository UserRepository => _userRepository ??= new UserRepository(context);
    
    private IProjectRepository? _projectRepository;
    public IProjectRepository ProjectRepository => _projectRepository ??= new ProjectRepository(context);
    
    private ITaskRepository? _taskRepository;
    public ITaskRepository TaskRepository => _taskRepository ??= new TaskRepository(context);
    
    public void BeginTransaction()
    {
        context.Database.BeginTransaction();
    }
    
    public async Task BeginTransactionAsync()
    {
        await context.Database.BeginTransactionAsync();
    }

    public void Commit()
    {
        context.Database.CommitTransaction();
    }

    public async Task CommitAsync()
    {
        await context.Database.CommitTransactionAsync();
    }
    
    public void Rollback()
    {
        context.Database.RollbackTransaction();
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