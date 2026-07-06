using MinimalAPIsTopLearn.DTOs;

namespace MinimalAPIsTopLearn.Utilities.Extentions;

public static class IQurableExtentions
{
    public static IQueryable<T> Paginate<T>(this IQueryable<T> querable, PaginationDTO paginationDTO)
    {
        return querable
            .Skip((paginationDTO.Page - 1) * paginationDTO.RecordsPerPage)
            .Take(paginationDTO.RecordsPerPage);
    }
}
