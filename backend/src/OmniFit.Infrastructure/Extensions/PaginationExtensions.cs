namespace OmniFit.Infrastructure.Extensions
{
    public static class PaginationExtensions
    {
        public static IQueryable<T> ApplyPagination<T> (this IQueryable<T> query, int page, int pageSize)
        {
            return query
                .Skip((page - 1) * pageSize)
                .Take(pageSize);
        }
    }
}