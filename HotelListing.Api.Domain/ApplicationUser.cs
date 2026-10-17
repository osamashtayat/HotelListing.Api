using System.ComponentModel.DataAnnotations.Schema;
using HotelListing.Api.Common.Constants;
using Microsoft.AspNetCore.Identity;
namespace HotelListing.Api.Domain;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; }=string.Empty;
    public string LastName { get; set; } = string.Empty;
    [NotMapped]
    public string  FullName=>$"{LastName},{FirstName}";

    public string Role { get; set; } = RoleNames.User;
}
