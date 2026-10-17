using System.ComponentModel.DataAnnotations;

namespace HotelListing.Api.Application.DTOs.Booking;

public record CreateBookingDto
(
  [property: Required] int HotelId,
   DateOnly CheckIn,
   DateOnly CheckOut,
  [property: Required] [property: Range(1, 10)] int Guests
): IValidatableObject
{
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (CheckIn > CheckOut)
        {
            yield return new ValidationResult("Check in and check out must be on different days", [ nameof(CheckIn), nameof(CheckOut)]);
        }
    }
}
