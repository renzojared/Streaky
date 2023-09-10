using Microsoft.EntityFrameworkCore;

namespace Streaky.Movies.Helper;

public static class HtttpContextExtensions
{
    public async static Task InsertParametersPagination<T> (this HttpContext httpContext, IQueryable<T> queryable, int quantityRecordsByPage)
    {
        double quantity = await queryable.CountAsync();
        double quantityPages = Math.Ceiling(quantity / quantityRecordsByPage);

        httpContext.Response.Headers.Add("quantityPages", quantityPages.ToString());
    }
}

