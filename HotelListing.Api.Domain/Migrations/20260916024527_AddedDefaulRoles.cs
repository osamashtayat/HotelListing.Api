using Microsoft.EntityFrameworkCore.Migrations;
using HotelListing.Api.Domain.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HotelListing.Api.Domain.Migrations
{
    /// <inheritdoc />
    public partial class AddedDefaulRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "58c316f9-0983-43ff-9787-72fda7af6972", "79cde387-f719-4b1f-8fd5-9bd35c409cdb", "User", "USER" },
                    { "abbabbc4-a39d-48be-a3b3-4ed661eb4eb8", "a52ff1ee-9063-4093-827a-c2760f3142f1", "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "58c316f9-0983-43ff-9787-72fda7af6972");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "abbabbc4-a39d-48be-a3b3-4ed661eb4eb8");
        }
    }
}
