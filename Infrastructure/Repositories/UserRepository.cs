using Domain.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserRepository(DbContext context) : Repository<UserEntity>(context), IUserRepository
{
}