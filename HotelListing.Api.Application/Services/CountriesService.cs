
using HotelListing.Api.Application.DTOs.Country;
namespace HotelListing.Api.Application.Services;
using HotelListing.Api.Domain;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using global::HotelListing.Api.Application.Contracts;
using global::HotelListing.Api.Application.DTOs.Hotel;
using HotelListing.Api.Common.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;


public class CountriesService(HotelListingDbContext context, IMapper mapper) : ICountriesService
{
    public async Task<Result<IEnumerable<GetCountriesDto>>> GetAllCountries()
    {
        var countries = await context.Countries.ProjectTo<GetCountriesDto>(mapper.ConfigurationProvider).ToListAsync();

        return Result<IEnumerable<GetCountriesDto>>.Success(countries);
    }
    public async Task<Result<GetCountryDto>> GetCountryById(int id)
    {
        var country = await context.Countries.Where(c => c.CountryId == id)
            .ProjectTo<GetCountryDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();

        return country is null ? Result<GetCountryDto>.NotFound() : Result<GetCountryDto>.Success(country);
    }
    public async Task<Result> UpdateCountry(int id, UpdateCountryDto countryDto)
    {
        try
        {
            if (id != countryDto.Id)
            {
                return Result.BadRequest(new Error("Invalid country id", "The provided country id does not match the country dto id."));
            }
            var country = await context.Countries.FindAsync(id);
            if (country is null)
            {
                return Result.NotFound(new Error("Country not found", $"Country with id {id} not found."));
            }
            var duplicateName = await CountryExistsByName(countryDto.Name);
            if (duplicateName)
            {
                return Result.Failure(new Error("Duplicate country name", "A country with the same name already exists."));
            }
        
            mapper.Map(countryDto, country);
            context.Countries.Update(country);
            await context.SaveChangesAsync();
            return Result.Success();

        }
        catch (Exception ex)
        {
            return Result.Failure(new Error("Error updating country", ex.Message));
        }


    }
    public async Task<Result<GetCountryDto>> CreateCountry(CreateCountryDto countryDto)
    {
        try
        {
            var exists = await CountryExistsByName(countryDto.Name);
            if (exists)
            {
                return Result<GetCountryDto>.Failure(new Error("Country already exists", "A country with the same name already exists."));
            }


            var country = new Country
            {
                Name = countryDto.Name,
                shortName = countryDto.ShortName
            };

            context.Countries.Add(country);
            await context.SaveChangesAsync();

            var resultDto = mapper.Map<GetCountryDto>(country);

            return Result<GetCountryDto>.Success(resultDto);
        }

        catch (Exception ex)
        {
            return Result<GetCountryDto>.Failure(new Error("Error creating country", ex.Message));
        }
    }



    public async Task<Result> DeleteCountry(int id)
    {
        try
        {
            var country = await context.Countries.FindAsync(id);
            if (country is null)
            {
                return Result.NotFound(new Error("Country not found", $"Country with id {id} not found."));
            }
            context.Countries.Remove(country);
            await context.SaveChangesAsync();
            return Result.Success();
        }

        catch (Exception ex)
        {
            return Result.Failure(new Error("Error deleting country", ex.Message));
        }
    }

    public async Task<Result<bool>> CountryExistsById(int id)
    {
        return Result<bool>.Success(await context.Countries.AnyAsync(e => e.CountryId == id));
    }
    public async Task<bool> CountryExistsByName(string name)
    {
        return await context.Countries.AnyAsync(e => e.Name.ToLower().Trim() == name.ToLower().Trim());
    }
}
