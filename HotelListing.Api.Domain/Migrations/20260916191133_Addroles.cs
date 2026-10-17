using Microsoft.EntityFrameworkCore.Migrations;
using HotelListing.Api.Domain.Migrations;

#nullable disable

namespace HotelListing.Api.Domain.Migrations
{
    /// <inheritdoc />
    public partial class Addroles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "58c316f9-0983-43ff-9787-72fda7af6972",
                column: "ConcurrencyStamp",
                value: "b9bf5cc8-ad5c-486a-9691-39225534818c");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "abbabbc4-a39d-48be-a3b3-4ed661eb4eb8",
                column: "ConcurrencyStamp",
                value: "97175c89-735c-4a98-ad0c-ac6d58968692");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "58c316f9-0983-43ff-9787-72fda7af6972",
                column: "ConcurrencyStamp",
                value: "79cde387-f719-4b1f-8fd5-9bd35c409cdb");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "abbabbc4-a39d-48be-a3b3-4ed661eb4eb8",
                column: "ConcurrencyStamp",
                value: "a52ff1ee-9063-4093-827a-c2760f3142f1");
        }
    }
}
