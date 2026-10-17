using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HotelListing.Api.Common.Constants;
using HotelListing.Api.Common.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using HotelListing.Api.Application.Contracts;
using HotelListing.Api.Domain;
using HotelListing.Api.Application.DTOs.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using HotelListing.Api.Common.Models;
using HotelListing.Api.Common.Models.Config;

namespace HotelListing.Api.Application.Services;



public class UserService(UserManager<ApplicationUser> userManager,IOptions<JwtSettings> jwtOptions,IHttpContextAccessor httpContextAccessor,HotelListingDbContext hotelListingDbContext) : IUserService
{
    public string UserId =>
     httpContextAccessor?.HttpContext?.User?
     .FindFirst(JwtRegisteredClaimNames.Sub)?.Value ??
    httpContextAccessor?.HttpContext?.User?
     .FindFirst(ClaimTypes.NameIdentifier)?.Value
    ?? string.Empty;
    
    public async Task<Result<RegisteredUserDto>> RegisterAsync(RegisterUserDto resgisterUserDto)
    {
        var user = new ApplicationUser
        {
            Email = resgisterUserDto.Email,
            FirstName = resgisterUserDto.FirstName,
            LastName = resgisterUserDto.LastName,
            UserName = resgisterUserDto.Email
        };
        var result = await userManager.CreateAsync(user, resgisterUserDto.Password);
        if (!result.Succeeded)

        {
            var errors = result.Errors.Select(e => new Error(ErrorCodes.BadRequest, e.Description)).ToArray();
            return Result<RegisteredUserDto>.BadRequest(errors);
        }
        await userManager.AddToRoleAsync(user, resgisterUserDto.Role);
        if(resgisterUserDto.Role == RoleNames.HotelAdmin)
        {
            var hotelAdmin = hotelListingDbContext.HotelAdmins.Add(new HotelAdmin
            {
                UserId = user.Id,
                HotelId = resgisterUserDto.AssociatedHotelId.GetValueOrDefault()
            }
            );
            await hotelListingDbContext.SaveChangesAsync();
        }
        var registeredUser = new RegisteredUserDto
        {
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Id = user.Id,
            Role = resgisterUserDto.Role
        };
        return Result<RegisteredUserDto>.Success(registeredUser);
    }
    public async Task<Result<string>> LoginAsync(LoginUserDto loginUserDto)
    {
        var user = await userManager.FindByEmailAsync(loginUserDto.Email);
        if (user is null)
            return Result<string>.Failure(new Error(ErrorCodes.BadRequest, "Invalid Credentials"));

        var isPasswordValid = await userManager.CheckPasswordAsync(user, loginUserDto.Password);
        if (!isPasswordValid)
            return Result<string>.Failure(new Error(ErrorCodes.BadRequest, "Invalid Credentials"));

        //Issue token
        var token = await GenerateTokenAsync(user);

        return Result<string>.Success(token);

    }

    private async Task<string> GenerateTokenAsync(ApplicationUser user)
    {
        // set basic user claims
        var claims = new List<Claim>
        {
            new (JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new (JwtRegisteredClaimNames.Email, user.Email),
            new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new (JwtRegisteredClaimNames.Name, user.FullName)

        };
        // set user role claims
        var roles = await userManager.GetRolesAsync(user);
        var roleClaims = roles.Select(r => new Claim(ClaimTypes.Role, r));
        claims = claims.Union(roleClaims).ToList();
        // set Jwt Key credential
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Value.Key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        // create an encoded token

        var token = new JwtSecurityToken(
        issuer: jwtOptions.Value.Issuer,
        audience: jwtOptions.Value.Audience,
        claims: claims,
        expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(jwtOptions.Value.DurationInMinutes)),
        signingCredentials: credentials );

        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
