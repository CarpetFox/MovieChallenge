using MovieChallenge.BLL.DTOs;

namespace MovieChallenge.BLL.Extensions
{
    public static class LinqExtensions
    {
        public static IQueryable<T> Paginate<T, TAny>(this IQueryable<T> query, PaginatedDataRequest<TAny> pagination)
        {
            return query.Skip(pagination.PageSize * pagination.Page)
                .Take(pagination.PageSize);
        }
    }
}
