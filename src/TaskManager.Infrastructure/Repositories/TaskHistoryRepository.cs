using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Infrastructure.Repositories;

public class TaskHistoryRepository(DbContext context) : Repository<TaskHistoryEntity>(context), ITaskHistoryRepository
{
}