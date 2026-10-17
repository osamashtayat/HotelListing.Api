using HotelListing.Api.Common.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelListing.Api.Domain.Configurations;

public class RoleConfigurations : IEntityTypeConfiguration<IdentityRole>
{
    public void Configure(EntityTypeBuilder<IdentityRole> builder)
    {
        builder.HasData(
            new IdentityRole
            {
                Id = "abbabbc4-a39d-48be-a3b3-4ed661eb4eb8",
                Name = RoleNames.Admin,
                NormalizedName = RoleNames.Admin.ToUpperInvariant(),
                ConcurrencyStamp = "cb42864c-6b35-4ed0-97a7-c842b982333e"
            },
            new IdentityRole
            {
                Id = "58c316f9-0983-43ff-9787-72fda7af6972",
                Name = RoleNames.User,
                NormalizedName = RoleNames.User.ToUpperInvariant(),
                ConcurrencyStamp = "4343a75d-1553-45a9-9fec-5a2459525e3f"
            },
            new IdentityRole
            {
                Id = "abbabbc4-0983-48be-9787-4ed661eb4eb8",
                Name = RoleNames.HotelAdmin,
                NormalizedName = RoleNames.HotelAdmin.ToUpperInvariant(),
                ConcurrencyStamp = "c756170c-43b9-4535-9cd7-5cdf7ab26790"
            }
        );
    }
}
