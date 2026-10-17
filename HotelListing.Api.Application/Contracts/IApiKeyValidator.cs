namespace HotelListing.Api.Application.Contracts;

public interface IApiKeyValidator
{
    Task<bool> IsValidAsync(string apiKey, CancellationToken cancellationToken);
}