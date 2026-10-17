using System.ComponentModel.DataAnnotations;
using HotelListing.Api.Application.DTOs.Hotel;

namespace HotelListing.Api.Application.DTOs.Hotel;

public class UpdateHotelDto :CreateHotelDto
{
    [Required]
    public int Id { get; set; }
}
