using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Infrastructure.Repositories;

public class TaskCommentRepository(DbContext context) : Repository<TaskCommentEntity>(context), ITaskCommentRepository
{
}