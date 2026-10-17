using HotelListing.Api.Common.Results;
using HotelListing.Api.Application.DTOs.Booking;
using HotelListing.Api.Domain;
using Azure;
using HotelListing.Api.Common.Models.Paging;
namespace HotelListing.Api.Application.Contracts
{
    public interface IBookingService
    {
        Task<Result<PagedResult<GetBookingDto>>> GetBookingForHotelAsync(int hotelId, PaginationParameters paginationParameters);
        Task<Result<GetBookingDto>> CreateBookingAsync(CreateBookingDto bookingDto);
        Task<Result<GetBookingDto>> UpdateBookingAsync(int hotelId, int bookingId, UpdateBookingDto bookingDto);
        Task<Result> CancelBookingForAdminAsync(int hotelId, int bookingId);
        Task<Result> ConfirmBookingForAdminAsync(int hotelId, int bookingId);
        Task<Result> CancelBookingAsync(int hotelId, int bookingId);
        Task<Result<PagedResult<GetBookingDto>>> GetUserBookingForHotelAsync(int hotelId, PaginationParameters paginationParameters);
    }
}
