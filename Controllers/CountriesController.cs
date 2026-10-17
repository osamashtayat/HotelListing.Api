using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotelListing.Api.Common.Constants;
using HotelListing.Api.Application.Contracts;
using HotelListing.Api.Domain;
using HotelListing.Api.Application.DTOs.Country;
using HotelListing.Api.Application.DTOs.Hotel;
using HotelListing.Api.Common.Results;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WebApplication1.Controllers;

[Route("api/[controller]")]
[ApiController]
    [Authorize]
public class CountriesController(ICountriesService countriesService) : BaseApiController
{

    // GET: api/Countries
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetCountriesDto>>> GetCountries()
    {
        var countries = await countriesService.GetAllCountries();

        return ToActionResult(countries);
    }

    // GET: api/Countries/5
    [HttpGet("{id}")]
    [Authorize(Roles = RoleNames.Admin)]
    public async Task<ActionResult<GetCountryDto>> GetCountry(int id)
    {
        var country = await countriesService.GetCountryById(id);
        
        return ToActionResult(country);}

    // PUT: api/Countries/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    [Authorize(Roles = RoleNames.Admin)]
    public async Task<IActionResult> PutCountry(int id, UpdateCountryDto countryDto)
    {
        var result = await countriesService.UpdateCountry(id, countryDto);

        //update country
        return ToActionResult(result);
    }

    // POST: api/Countries
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    [Authorize(Roles = RoleNames.Admin)]
    public async Task<ActionResult<Country>> PostCountry(CreateCountryDto countryDto)
    {
            // create country
            var resultDto = await countriesService.CreateCountry(countryDto);
            if(!resultDto.IsSuccess)
            {
                return MapErrorsToResponse(resultDto.Errors);
            }
            return CreatedAtAction(nameof(GetCountry), new { id = resultDto.Value!.Id }, resultDto.Value);
    }

    // DELETE: api/Countries/5
    [HttpDelete("{id}")]
    [Authorize(Roles = RoleNames.Admin)]
    public async Task<IActionResult> DeleteCountry(int id)
    {
        var result = await countriesService.DeleteCountry(id);
        return ToActionResult(result);
    }

  

}
