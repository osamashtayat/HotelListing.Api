using Microsoft.EntityFrameworkCore.Migrations;
using HotelListing.Api.Domain.Migrations;

#nullable disable

namespace HotelListing.Api.Domain.Migrations
{
    /// <inheritdoc />
    public partial class FixHotelRelationshipAndSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Hotels_HotelId1",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_HotelId1",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "HotelId1",
                table: "Bookings");

            migrationBuilder.AlterColumn<int>(
                name: "HotelId",
                table: "Bookings",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Hotels_HotelId",
                table: "Bookings",
                column: "HotelId",
                principalTable: "Hotels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Hotels_HotelId",
                table: "Bookings");

            migrationBuilder.AlterColumn<string>(
                name: "HotelId",
                table: "Bookings",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "HotelId1",
                table: "Bookings",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_HotelId1",
                table: "Bookings",
                column: "HotelId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Hotels_HotelId1",
                table: "Bookings",
                column: "HotelId1",
                principalTable: "Hotels",
                principalColumn: "Id");
        }
    }
}
