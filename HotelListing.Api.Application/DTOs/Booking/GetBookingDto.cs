using HotelListing.Api.Domain;

namespace HotelListing.Api.Application.DTOs.Booking;
public record GetBookingDto
(
    int Id,
    int HotelId,
    string name,
    DateOnly CheckIn,
    DateOnly CheckOut,
    int Guests,
    decimal TotalPrice,
    string Status,
    DateTime CreatedAt,
    DateTime UpdatedAt
);
