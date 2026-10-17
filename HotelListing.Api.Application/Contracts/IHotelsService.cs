using HotelListing.Api.Common.Results;
using HotelListing.Api.Application.DTOs.Hotel;
namespace HotelListing.Api.Application.Contracts;

public interface IHotelsService
{
    Task<Result<GetHotelDto>> CreateHotel(CreateHotelDto hotelDto);
    Task<Result> DeleteHotel(int id);
    Task<Result<IEnumerable<GetHotelDto>>> GetAllHotels();
    Task<Result<GetHotelDto?>> GetHotelById(int id);
    Task<bool> HotelExists(int id);
    Task<bool> HotelExists(string name);
    Task<Result> UpdateHotel(int id, UpdateHotelDto hotelDto);
}