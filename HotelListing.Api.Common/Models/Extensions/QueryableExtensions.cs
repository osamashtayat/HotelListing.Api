using HotelListing.Api.Common.Models.Paging;
using Microsoft.EntityFrameworkCore;
namespace HotelListing.Api.Common.Models.Extensions;

public static class QueryableExtensions
{
    public static async Task<PagedResult<T>> ToPagedResultAsync<T>
    (this IQueryable<T> source, PaginationParameters paginationParameters)
    {
        var totalCount = await source.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)paginationParameters.PageSize);

        var data = await source
            .Skip((paginationParameters.PageNumber - 1) * paginationParameters.PageSize)
            .Take(paginationParameters.PageSize)
            .ToListAsync();
        var metadata = new PaginationMetadata
        {
            CurrentPage = paginationParameters.PageNumber,
            PageSize = paginationParameters.PageSize,
            TotalPages = totalPages,
            TotalCount = totalCount,
            HasPrevious = paginationParameters.PageNumber > 1,
            HasNext = paginationParameters.PageNumber < totalPages
        };
        return new PagedResult<T>
        {
            Data = data,
            Metadata = metadata
        };
    }
}
