using MinimalAPIsTopLearn.DTOs;

namespace MinimalAPIsTopLearn.Utilities.Extentions;

public static class IQueryableExtentions
{
    public static IQueryable<T> Paginate<T>(this IQueryable<T> queryable, PaginationDTO paginationDTO)
    {
        return queryable
            .Skip((paginationDTO.Page - 1) * paginationDTO.RecordsPerPage)
            .Take(paginationDTO.RecordsPerPage);
    }
}
