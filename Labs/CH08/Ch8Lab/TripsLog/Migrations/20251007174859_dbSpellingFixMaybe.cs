using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TripsLog.Migrations
{
    /// <inheritdoc />
    public partial class dbSpellingFixMaybe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AccomodationPhone",
                table: "Trips",
                newName: "AccommodationPhone");

            migrationBuilder.RenameColumn(
                name: "AccomodationEmail",
                table: "Trips",
                newName: "AccommodationEmail");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AccommodationPhone",
                table: "Trips",
                newName: "AccomodationPhone");

            migrationBuilder.RenameColumn(
                name: "AccommodationEmail",
                table: "Trips",
                newName: "AccomodationEmail");
        }
    }
}
