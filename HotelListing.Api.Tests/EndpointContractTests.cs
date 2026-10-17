using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using WebApplication1.Controllers;

namespace HotelListing.Api.Tests;

public sealed class EndpointContractTests
{
    [Fact]
    public void AuthController_IsAnonymousAndUsesExpectedRoute()
    {
        Assert.NotNull(Attribute.GetCustomAttribute(typeof(AuthController), typeof(AllowAnonymousAttribute)));
        Assert.Equal("api/[controller]", GetRoute<AuthController>());
    }

    [Fact]
    public void CountriesController_RequiresAuthentication()
    {
        Assert.NotNull(Attribute.GetCustomAttribute(typeof(CountriesController), typeof(AuthorizeAttribute)));
    }

    [Fact]
    public void BookingController_RequiresAuthenticationAndHotelRoute()
    {
        Assert.NotNull(Attribute.GetCustomAttribute(typeof(HotelBookingController), typeof(AuthorizeAttribute)));
        Assert.Equal("api/hotels/{hotelId:int}/bookings", GetRoute<HotelBookingController>());
    }

    [Fact]
    public void HotelsController_RemainsPublic()
    {
        Assert.Null(Attribute.GetCustomAttribute(typeof(HotelsController), typeof(AuthorizeAttribute)));
        Assert.Equal("api/[controller]", GetRoute<HotelsController>());
    }

    [Theory]
    [InlineData(typeof(AuthController), "Login", "login")]
    [InlineData(typeof(AuthController), "Register", "register")]
    [InlineData(typeof(HotelBookingController), "GetBookingsAdmin", "admin")]
    public void EndpointActions_ExposeExpectedTemplates(Type controller, string method, string template)
    {
        var methodInfo = controller.GetMethod(method)!;
        var routeTemplate = methodInfo.GetCustomAttributes(typeof(HttpMethodAttribute), true)
            .Cast<HttpMethodAttribute>().Single().Template;

        Assert.Equal(template, routeTemplate);
    }

    private static string? GetRoute<T>() =>
        typeof(T).GetCustomAttributes(typeof(RouteAttribute), true)
            .Cast<RouteAttribute>().Single().Template;
}
