using Microsoft.EntityFrameworkCore;

namespace Streaky.Udemy.Utilities;

public static class HttpContextExtension
{
    public async static Task InsertParameteresPaginationInHeader<T>(this HttpContext httpContext,
        IQueryable<T> queryable)
    {
        if (httpContext == null) { throw new ArgumentNullException(nameof(httpContext)); }

        double quantity = await queryable.CountAsync();
        httpContext.Response.Headers.Add("quantityTotalRecords", quantity.ToString());
    }
}

