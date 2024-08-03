using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class TaskCommentRepository(DbContext context) : Repository<TaskCommentEntity>(context), ITaskCommentRepository
{
}