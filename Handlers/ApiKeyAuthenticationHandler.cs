using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using HotelListing.Api.Common.Constants;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Options;
using HotelListing.Api.Domain;
using HotelListing.Api.Application.Contracts;
using HotelListing.Api.Application.DTOs.Auth;
using HotelListing.Api.Application.Services;
using static System.Net.Mime.MediaTypeNames;
namespace WebApplication1.Handlers;

public class ApiKeyAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,ILoggerFactory logger,UrlEncoder encoder, IApiKeyValidator apiKeyValidtor ) : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        string apiKey = string.Empty;
        if(Request.Headers.TryGetValue(AuthenticationDefaults.ApiKeyHeader, out var apiKeyHeader))
        {
            apiKey = apiKeyHeader.ToString().Trim();
        }
        if(string.IsNullOrEmpty(apiKey))
        {
            return AuthenticateResult.Fail("Invalid credentials");
        }
        var result = await apiKeyValidtor.IsValidAsync(apiKey, Context.RequestAborted);
        if (!result)
        {
            return AuthenticateResult.Fail("Invalid credentials");
        }
        var claims = new List<Claim>
        {
            new (ClaimTypes.NameIdentifier, "apikey"),
            new (ClaimTypes.Name, "ApiKeyClient")
        };
        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);
        return AuthenticateResult.Success(ticket);
    }
}   
