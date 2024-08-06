using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Infrastructure.Repositories;

public class UserRepository(DbContext context) : Repository<UserEntity>(context), IUserRepository
{
}