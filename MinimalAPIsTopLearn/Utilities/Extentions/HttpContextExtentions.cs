using Microsoft.EntityFrameworkCore;

namespace MinimalAPIsTopLearn.Utilities.Extentions;

public static class HttpContextExtentions
{
    public async static Task InsertPaginationInfoIntoResponseHeader<T>(this HttpContext httpContext,
        IQueryable<T> queryable)
    {
        ArgumentNullException.ThrowIfNull(httpContext);

        int count = await queryable.CountAsync();
        httpContext.Response.Headers.Append("totalamountofrecords", count.ToString());
    }
}
