using Streaky.Udemy.DTOs;

namespace Streaky.Udemy.Utilities;

public static class IQueryableExtension
{
    public static IQueryable<T> ToPage<T>(this IQueryable<T> queryable, PaginationDTO paginationDTO)
    {
        return queryable.Skip((paginationDTO.Page - 1) * paginationDTO.RecordsByPage)
            .Take(paginationDTO.RecordsByPage);
    }
}

