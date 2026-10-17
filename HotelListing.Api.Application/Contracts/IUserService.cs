
using HotelListing.Api.Common.Results;
using Microsoft.AspNetCore.Mvc;
using HotelListing.Api.Application.DTOs.Auth;
namespace HotelListing.Api.Application.Contracts;
public interface IUserService
{
    string UserId { get; }
    Task<Result<string>> LoginAsync(LoginUserDto loginUserDto);
    Task<Result<RegisteredUserDto>> RegisterAsync(RegisterUserDto resgisterUserDto);
}
