using HotelListing.Api.Common.Results;
using Microsoft.AspNetCore.Mvc;
using HotelListing.Api.Application.DTOs.Country;

namespace HotelListing.Api.Application.Contracts;

public interface ICountriesService
{
    Task<Result<bool>> CountryExistsById(int id);
    Task<bool> CountryExistsByName(string name);
    Task<Result<GetCountryDto>> CreateCountry(CreateCountryDto countryDto);
    Task<Result> DeleteCountry(int id);
    Task<Result<IEnumerable<GetCountriesDto>>> GetAllCountries();
    Task<Result<GetCountryDto>> GetCountryById(int id);
    Task<Result> UpdateCountry(int id, UpdateCountryDto countryDto);
}

