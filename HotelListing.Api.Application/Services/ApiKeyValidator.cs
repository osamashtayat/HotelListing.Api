using Microsoft.EntityFrameworkCore;
using HotelListing.Api.Application.Contracts;
using HotelListing.Api.Domain;
namespace HotelListing.Api.Application.Services;
public class ApiKeyValidator(HotelListingDbContext db) : IApiKeyValidator
{
    public async Task<bool> IsValidAsync(string apiKey, CancellationToken ct=default)

    {

        if (string.IsNullOrEmpty(apiKey))
        {
            return false;
        }
        var apiKeyEntity = await db.ApiKeys.AsNoTracking().FirstOrDefaultAsync(x => x.Key == apiKey, ct);

        if (apiKeyEntity is null) return false;
        return apiKeyEntity.IsActive;

    }

}
