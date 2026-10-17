

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Azure;
using HotelListing.Api.Common.Constants;
using HotelListing.Api.Common.Models.Paging;
using HotelListing.Api.Common.Results;
using HotelListing.Api.Application.DTOs.Booking;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using HotelListing.Api.Application.Contracts;
using HotelListing.Api.Domain;
using HotelListing.Api.Domain.Enums;
using HotelListing.Api.Common.Models.Extensions;

namespace HotelListing.Api.Application.Services;

public class BookingService(HotelListingDbContext context, IUserService userService, IMapper mapper) : IBookingService
{
    public async Task<Result<PagedResult<GetBookingDto>>> GetBookingForHotelAsync(int hotelId, PaginationParameters paginationParameters)

    {
        var hotelExists = await context.Hotels.AnyAsync(h => h.Id == hotelId);
        if (!hotelExists)
        {
            return Result<PagedResult<GetBookingDto>>.Failure(new Error(ErrorCodes.NotFound, "Hotel not found"));
        }
        var bookings = await context.Bookings.
        Where(b => b.HotelId == hotelId).
        OrderBy(b => b.CheckIn)
        .ProjectTo<GetBookingDto>(mapper.ConfigurationProvider)
        .ToPagedResultAsync(paginationParameters);
        return Result<PagedResult<GetBookingDto>>.Success(bookings);
    }
    public async Task<Result<PagedResult<GetBookingDto>>> GetUserBookingForHotelAsync(int hotelId, PaginationParameters paginationParameters)
    {
        var userId = userService.UserId;
        var hotelExists = await context.Hotels.AnyAsync(h => h.Id == hotelId);
        if (!hotelExists)
        {
            return Result<PagedResult<GetBookingDto>>.Failure(new Error(ErrorCodes.NotFound, "Hotel not found"));
        }
        var bookings = await context.Bookings.
        Where(b => b.HotelId == hotelId&&b.UserId==userId).
        OrderBy(b => b.CheckIn)
        .ProjectTo<GetBookingDto>(mapper.ConfigurationProvider)
        .ToPagedResultAsync(paginationParameters);
        return Result<PagedResult<GetBookingDto>>.Success(bookings);
            }
    public async Task<Result<GetBookingDto>> CreateBookingAsync(CreateBookingDto bookingDto)
    {
        var userId = userService.UserId;
         var overlabs = await IsOverlap(bookingDto.HotelId, userId, bookingDto.CheckIn, bookingDto.CheckOut);
        if (overlabs)
        {
            return Result<GetBookingDto>.Failure(new Error("Booking overlaps with existing booking", "Booking overlaps with existing booking"));
        }
        var hotel = await context.Hotels.Where(h => h.Id == bookingDto.HotelId).FirstOrDefaultAsync();
        if (hotel is null)
        {
            return Result<GetBookingDto>.Failure(new Error("Hotel not found", "Hotel not found"));
        }
        var nights = bookingDto.CheckOut.DayNumber - bookingDto.CheckIn.DayNumber;
        var totalPrice = hotel.PerNightRate * nights;
        var booking = mapper.Map<Booking>(bookingDto);
        booking.UserId = userId;
        booking.TotalPrice = totalPrice;
        context.Bookings.Add(booking);
        await context.SaveChangesAsync();
        var result = mapper.Map<GetBookingDto>(booking);
        return Result<GetBookingDto>.Success(result);

    }
    public async Task<Result<GetBookingDto>> UpdateBookingAsync([FromRoute] int hotelId, [FromRoute] int bookingId, [FromBody] UpdateBookingDto bookingDto)
    {
        var userId = userService.UserId;
        var overlabs = await IsOverlap(hotelId, userId, bookingDto.CheckIn, bookingDto.CheckOut, bookingId);
        if (overlabs)
        {
            return Result<GetBookingDto>.Failure(new Error("Booking overlaps with existing booking", "Booking overlaps with existing booking"));
        }
        var booking = await context.Bookings.Include(b => b.Hotel).FirstOrDefaultAsync(b => b.Id == bookingId && b.HotelId == hotelId && b.UserId == userId);
        if (booking is null)
        {
            return Result<GetBookingDto>.Failure(new Error("Booking not found", "Booking not found"));
        }
        if (booking.Status == BookingStatus.Cancelled)
        {
            return Result<GetBookingDto>.Failure(new Error("Booking is cancelled", "Booking is cancelled"));
        }


        mapper.Map(bookingDto, booking);
        var nights = bookingDto.CheckOut.DayNumber - bookingDto.CheckIn.DayNumber;
        var perNightRate = booking.Hotel!.PerNightRate;
        booking.TotalPrice = perNightRate * nights;
        booking.UpdatedAt = DateTime.UtcNow;
        await context.SaveChangesAsync();
        var updated = mapper.Map<GetBookingDto>(booking);
        return Result<GetBookingDto>.Success(updated);
    }


private Task<bool> IsOverlap(int hotelId, string userId, DateOnly checkIn, DateOnly checkOut, int? bookingId=null)
    {
        var query = context.Bookings.Where(
            b => b.HotelId == hotelId &&
        b.CheckIn < checkOut &&
         b.CheckOut > checkIn
         && b.Status != BookingStatus.Cancelled
         && b.UserId == userId
        ).AsQueryable();
        if(bookingId.HasValue)
        {
            query = query.Where(b => b.Id != bookingId.Value);
        }
        return query.AnyAsync();
    }
    public async Task<Result> CancelBookingAsync(int hotelId, int bookingId)
    {
                    string? userId = userService.UserId;
        if (string.IsNullOrEmpty(userId))
        {
            return Result.Failure(new Error("User not found", "User not found"));
        }
        var booking = await context.Bookings.Include(b => b.Hotel).FirstOrDefaultAsync(b => b.Id == bookingId && b.HotelId == hotelId && b.UserId == userId);
        if (booking is null)
        {
            return Result.Failure(new Error(ErrorCodes.NotFound, "Booking not found"));
        }
        if (booking.Status == BookingStatus.Cancelled)
        {
            return Result.Failure(new Error(ErrorCodes.Conflict, "Booking is already cancelled"));
        }
        booking.Status = BookingStatus.Cancelled;
        booking.UpdatedAt = DateTime.UtcNow;
        await context.SaveChangesAsync();
        return Result.Success();
    }
    public async Task<Result> CancelBookingForAdminAsync(int hotelId, int bookingId)
    {
                string? userId = userService.UserId;
        if (string.IsNullOrEmpty(userId))
        {
            return Result.Failure(new Error("User not found", "User not found"));
        }
        var isHotelAdmin = await context.HotelAdmins.AnyAsync(h => h.HotelId == hotelId && h.UserId == userId);
        if(!isHotelAdmin)
        {
            return Result.Failure(new Error(ErrorCodes.Forbidden, "User is not authorized to cancel booking for this hotel"));
        }
        var booking = await context.Bookings.Include(b => b.Hotel).FirstOrDefaultAsync(b => b.Id == bookingId && b.HotelId == hotelId && b.User!.Role == RoleNames.Admin);
        if (booking is null)
        {
            return Result.Failure(new Error(ErrorCodes.NotFound, "Booking not found"));
        }
        if (booking.Status == BookingStatus.Cancelled)
        {
            return Result.Failure(new Error(ErrorCodes.Conflict, "Booking is already cancelled"));
        }
        booking.Status = BookingStatus.Cancelled;
        booking.UpdatedAt = DateTime.UtcNow;
        await context.SaveChangesAsync();
        return Result.Success();
    }
    public async Task<Result> ConfirmBookingForAdminAsync(int hotelId, int bookingId)
    {

        string? userId = userService.UserId;
        if (string.IsNullOrEmpty(userId))
        {
            return Result.Failure(new Error("User not found", "User not found"));
        }
         var isHotelAdmin = await context.HotelAdmins.AnyAsync(h => h.HotelId == hotelId && h.UserId == userId);
        if(!isHotelAdmin)
        {
            return Result.Failure(new Error(ErrorCodes.Forbidden, "User is not authorized to cancel booking for this hotel"));
        }
        var booking = await context.Bookings.Include(b => b.Hotel).FirstOrDefaultAsync(b => b.Id == bookingId && b.HotelId == hotelId && b.User!.Role == RoleNames.Admin);
        if (booking is null)
        {
            return Result.Failure(new Error(ErrorCodes.NotFound, "Booking not found"));
        }
        if (booking.Status == BookingStatus.Confirmed)
        {
            return Result.Failure(new Error(ErrorCodes.Conflict, "Booking is already confirmed"));
        }
        booking.Status = BookingStatus.Confirmed;
        booking.UpdatedAt = DateTime.UtcNow;
        await context.SaveChangesAsync();
        return Result.Success();

    }



}
