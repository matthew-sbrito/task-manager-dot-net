using Microsoft.EntityFrameworkCore;
using Domain.Interfaces;
using Domain.Entities;

namespace Infrastructure.Repositories;

public class TaskRepository(DbContext context) : Repository<TaskEntity>(context), ITaskRepository
{
}