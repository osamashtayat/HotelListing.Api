
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using HotelListing.Api.Application.Contracts;
using HotelListing.Api.Domain;
using HotelListing.Api.Application.DTOs.Auth;
using HotelListing.Api.Common.Results;
namespace WebApplication1.Controllers;

[Route("api/[controller]")]
[ApiController]
    [AllowAnonymous]

public class AuthController(IUserService userService) : BaseApiController
{
    [HttpPost("register")]
    public async Task<ActionResult<RegisteredUserDto>> Register(RegisterUserDto resgisterUserDto)
    {
        var result = await userService.RegisterAsync(resgisterUserDto);
        
        return ToActionResult(result);

    }
     [HttpPost("login")]
    public async Task<ActionResult<string>> Login(LoginUserDto loginUserDto)
    {
        var result =await userService.LoginAsync(loginUserDto);

            return ToActionResult(result);
 
    }

}
