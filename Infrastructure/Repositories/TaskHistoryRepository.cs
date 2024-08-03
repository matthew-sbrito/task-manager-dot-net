using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class TaskHistoryRepository(DbContext context) : Repository<TaskHistoryEntity>(context), ITaskHistoryRepository
{
}