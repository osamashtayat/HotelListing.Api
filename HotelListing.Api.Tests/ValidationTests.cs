using System.ComponentModel.DataAnnotations;
using HotelListing.Api.Application.DTOs.Auth;
using HotelListing.Api.Application.DTOs.Booking;
using HotelListing.Api.Common.Constants;

namespace HotelListing.Api.Tests;

public sealed class ValidationTests
{
    [Fact]
    public void Booking_CheckOutBeforeCheckIn_IsInvalid()
    {
        var dto = new CreateBookingDto(
            1,
            new DateOnly(2026, 10, 20),
            new DateOnly(2026, 10, 19),
            2);

        var errors = Validate(dto);

        Assert.Contains(errors, error => error.MemberNames.Contains(nameof(dto.CheckIn)));
    }

    [Fact]
    public void Booking_GuestsOutsideRange_IsInvalid()
    {
        var dto = new CreateBookingDto(
            1,
            new DateOnly(2026, 10, 20),
            new DateOnly(2026, 10, 22),
            11);

        Assert.Contains(Validate(dto), error => error.MemberNames.Contains(nameof(dto.Guests)));
    }

    [Fact]
    public void HotelAdmin_WithoutHotel_IsInvalid()
    {
        var dto = new RegisterUserDto
        {
            Email = "admin@example.com",
            Password = "Valid1!Password",
            FirstName = "Hotel",
            LastName = "Admin",
            Role = RoleNames.HotelAdmin
        };

        Assert.Contains(Validate(dto), error =>
            error.MemberNames.Contains(nameof(dto.AssociatedHotelId)));
    }

    [Fact]
    public void NormalUser_WithoutHotel_IsValid()
    {
        var dto = new RegisterUserDto
        {
            Email = "user@example.com",
            Password = "Valid1!Password",
            FirstName = "Normal",
            LastName = "User",
            Role = RoleNames.User
        };

        Assert.Empty(Validate(dto));
    }

    private static List<ValidationResult> Validate(object value)
    {
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(value, new ValidationContext(value), results, true);
        return results;
    }
}
