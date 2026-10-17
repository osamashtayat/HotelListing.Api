using HotelListing.Api.Common.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using HotelListing.Api.Domain;

namespace WebApplication1.AuthorizationFilters;
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
public sealed class HotelOrSystemAdminAttribute : TypeFilterAttribute
{
    public HotelOrSystemAdminAttribute() : base(typeof(HotelOrSystemAdminFilter))
    {
    }
}
public class HotelOrSystemAdminFilter(HotelListingDbContext dbcontext) : IAsyncAuthorizationFilter
{
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var httpUser = context.HttpContext.User;
        if (httpUser?.Identity?.IsAuthenticated == false)
        {
            context.Result = new UnauthorizedResult();
            return;
        }
        // Check if the user is in the "Admin" role globally
        if (httpUser!.IsInRole(RoleNames.Admin))
        {
            return;
        }
        var userId = httpUser.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            context.Result = new ForbidResult();
            return;
        }
        context.RouteData.Values.TryGetValue("hotelId", out var hotelIdObj);
        int.TryParse(hotelIdObj?.ToString(), out var hotelId);
        if (hotelId == 0)
        {
            context.Result = new ForbidResult();
            return;
        }
        var isHotelAdmin=await dbcontext.HotelAdmins.AnyAsync(h => h.HotelId == hotelId && h.UserId == userId);
        if (!isHotelAdmin)
        {
            context.Result = new ForbidResult();
            return;
        }
    }
}
