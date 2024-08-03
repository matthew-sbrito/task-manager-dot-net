using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Extensions;

public static class QueryableExtension
{
    public static IQueryable<TEntity> ApplyIncludes<TEntity>(this IQueryable<TEntity> queryable,
        IEnumerable<string> includes) where TEntity : class
    {
        return includes
            .Aggregate(queryable, (current, include) => current.Include(include));
    }
}