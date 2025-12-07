using App.Common.Response;
using Microsoft.EntityFrameworkCore;

namespace App.Common.Extensions
{
    public static class QueryableExtensions
    {
        public static async Task<PaginationResponse<T>> ToPaginatedListAsync<T>(
                    this IQueryable<T> source,
                    int pageNumber,
                    int pageSize,
                    CancellationToken ct = default)
        {
            var totalCount = await source.CountAsync(ct);

            var items = await source
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);

            return new PaginationResponse<T>
            {
                TotalCount = totalCount,
                PageSize = pageSize,
                PageNumber = pageNumber,
                Result = items
            };
        }

    }
}
