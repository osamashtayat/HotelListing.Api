using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace HotelListing.Api.Domain;
public class ApiKey
{
    public int Id { get; set; }
    [MaxLength(200)]
    public string Key { get; set; } = string.Empty;
    public string AppName { get; set; } = string.Empty;
    public DateTimeOffset? CreatedAtUtc { get; set; }
    public DateTimeOffset? ExpiresAtUtc { get; set; }
    
    [NotMapped]
    public bool IsActive => !ExpiresAtUtc.HasValue || ExpiresAtUtc > DateTimeOffset.UtcNow;

}