
using System.ComponentModel.DataAnnotations;

public record UpdateBookingDto
(
   DateOnly CheckIn,
   DateOnly CheckOut,
  [Required][Range(1, 10)] int Guests
) : IValidatableObject
{
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (CheckIn > CheckOut)
        {
            yield return new ValidationResult("Check in must be before check out", [ nameof(CheckIn), nameof(CheckOut)]);
        }
    }
}