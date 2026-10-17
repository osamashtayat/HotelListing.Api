using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using HotelListing.Api.Domain;
using static System.Net.Mime.MediaTypeNames;

namespace HotelListing.Api.Domain;

public class HotelListingDbContext(DbContextOptions<HotelListingDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Hotel> Hotels { get; set; }
    public DbSet<Country> Countries { get; set; }

    public DbSet<ApiKey> ApiKeys { get; set; }
    public DbSet<HotelAdmin> HotelAdmins { get; set; }
    public DbSet<Booking> Bookings { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<ApiKey>(b =>
        {
            b.HasIndex(x => x.Key).IsUnique();
        });
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }




}
