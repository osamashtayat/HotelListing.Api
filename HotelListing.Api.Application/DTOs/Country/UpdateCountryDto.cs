using System.ComponentModel.DataAnnotations;
using HotelListing.Api.Application.DTOs.Country;

namespace HotelListing.Api.Application.DTOs.Country;

public class UpdateCountryDto : CreateCountryDto
{
    [Required]
    public int Id { get; set; }
}
