using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Options;
using System.Text;
using HotelListing.Api.Domain;
using HotelListing.Api.Application.DTOs.Auth;
using static System.Net.Mime.MediaTypeNames;
using HotelListing.Api.Application.Contracts;
namespace WebApplication1.Handlers;

public class BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,ILoggerFactory logger,UrlEncoder encoder, IUserService userService) : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue("Authorization", out var authHeader))
        {
            return AuthenticateResult.NoResult();

        }

        var authHeaderValue = authHeader.ToString().Trim();
        if(string.IsNullOrEmpty(authHeaderValue)|| !authHeaderValue.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
        {
            return AuthenticateResult.NoResult();
        }
        var token = authHeaderValue["Basic ".Length..].Trim();
        string decoded;
        try
        {
            var crednetialBytes = Convert.FromBase64String(token);
            decoded = Encoding.UTF8.GetString(crednetialBytes);
        }
        catch{
            return AuthenticateResult.Fail("Invalid credentials");
        }
        var separatorIndex = decoded.IndexOf(':');
        if (separatorIndex <= 0)
        {
            return AuthenticateResult.Fail("Invalid credentials");
        }

        var userNameorEmail = decoded[..separatorIndex];
        var password = decoded[(separatorIndex + 1)..];
        var loginDto = new LoginUserDto
        {
            Email = userNameorEmail,
            Password = password
        };
        var result = await userService.LoginAsync(loginDto);
        if (!result.IsSuccess)
        {
            return AuthenticateResult.Fail("Invalid credentials");
        }
        var claims = new List<Claim>
        {
            new (ClaimTypes.Name, userNameorEmail),
        };
        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);
        return AuthenticateResult.Success(ticket);
    }
}   
