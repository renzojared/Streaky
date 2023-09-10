using Streaky.Movies.DTOs;

namespace Streaky.Movies.Helper;

public static class QueryableExtension
{
    public static IQueryable<T> Paginate<T>(this IQueryable<T> queryable, PaginationDTO paginationDTO)
    {
        return queryable
            .Skip((paginationDTO.Page - 1) * paginationDTO.QuantityRecordByPage)
            .Take(paginationDTO.QuantityRecordByPage);
    }
}

