using MovieChallenge.BLL.DTOs;
using System.Linq.Expressions;

namespace MovieChallenge.BLL.Extensions
{
    public static class LinqExtensions
    {
        public static IQueryable<TSource> Paginate<TSource, TAny>(this IQueryable<TSource> query, PaginatedDataRequest<TAny> pagination)
        {
            return query.Skip(pagination.PageSize * pagination.Page)
                .Take(pagination.PageSize);
        }

        public static IQueryable<TSource> OrderBy<TSource, TKey>(this IQueryable<TSource> query, Expression<Func<TSource, TKey>> keySelector, bool ascending)
        {
            return ascending ? query.OrderBy(keySelector) : query.OrderByDescending(keySelector);
        }
    }
}
