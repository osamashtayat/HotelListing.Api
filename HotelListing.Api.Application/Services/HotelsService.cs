using HotelListing.Api.Domain;

using HotelListing.Api.Application.DTOs.Hotel;
namespace HotelListing.Api.Application.Services;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using global::HotelListing.Api.Application.Contracts;
using global::HotelListing.Api.Application.DTOs.Hotel;
using HotelListing.Api.Common.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;



public class HotelsService(HotelListingDbContext context, IMapper mapper) : IHotelsService
{
    public async Task<Result<IEnumerable<GetHotelDto>>> GetAllHotels()
    {
        var hotels = await context.Hotels.ProjectTo<GetHotelDto>(mapper.ConfigurationProvider).ToListAsync();

        return Result<IEnumerable<GetHotelDto>>.Success(hotels);
    }
    public async Task<Result<GetHotelDto?>> GetHotelById(int id)
    {
        var hotel = await context.Hotels.Where(h => h.Id == id)
        .Include(h => h.Country)
            .ProjectTo<GetHotelDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();

        return Result<GetHotelDto?>.Success(hotel);
    }
    public async Task<Result> UpdateHotel(int id, UpdateHotelDto hotelDto)
    {
        try
        {
            if (id != hotelDto.Id)
            {
                return Result.BadRequest(new Error("Invalid hotel id", "The provided hotel id does not match the hotel dto id."));
            }
            var hotel = await context.Hotels.FindAsync(id);
            if (hotel is null)
            {
                return Result.NotFound(new Error("Hotel not found", $"Hotel with id {id} not found."));
            }
            var duplicateName = await HotelExists(hotelDto.Name);
            if (duplicateName)
            {
                return Result.Failure(new Error("Duplicate hotel name", "A hotel with the same name already exists."));
            }

            mapper.Map(hotelDto, hotel);
            context.Hotels.Update(hotel);
            await context.SaveChangesAsync();
        }

        catch (Exception ex)
        {
            return Result.Failure(new Error("Error updating hotel", ex.Message));
        }

        return Result.Success();

    }
    public async Task<Result<GetHotelDto>> CreateHotel(CreateHotelDto hotelDto)
    {
        try
        {
            if (await context.Hotels.AnyAsync(h => h.Name == hotelDto.Name))
            {
                return Result<GetHotelDto>.Failure(new Error("Hotel already exists", "A hotel with the same name already exists."));
            }
            var hotel = mapper.Map<Hotel>(hotelDto);
            context.Hotels.Add(hotel);
            await context.SaveChangesAsync();
            var returnObj = mapper.Map<GetHotelDto>(hotel);
            return Result<GetHotelDto>.Success(returnObj);
        }

        catch (Exception ex)
        {
            return Result<GetHotelDto>.Failure(new Error("Error creating hotel", ex.Message));
        }
    }

    public async Task<Result> DeleteHotel(int id)
    {
        var hotel = await context.Hotels.Where(q => q.Id == id).ExecuteDeleteAsync();
        if (hotel == 0)
        {
            return Result.NotFound(new Error("Hotel not found", $"Hotel with id {id} not found."));
        }
        return Result.Success();
    }

    public async Task<bool> HotelExists(int id)
    {
        return await context.Hotels.AnyAsync(e => e.Id == id);
    }
    public async Task<bool> HotelExists(string name)
    {
        return await context.Hotels.AnyAsync(e => e.Name == name);
    }
}