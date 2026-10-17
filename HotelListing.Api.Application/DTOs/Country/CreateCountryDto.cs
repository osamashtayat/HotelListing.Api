using System.ComponentModel.DataAnnotations;

namespace HotelListing.Api.Application.DTOs.Country;

public class CreateCountryDto
{
    [Required]
    public required string Name { get; set; }

    [Required]
    public required string ShortName { get; set; }
}
