using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TripsLog.Migrations
{
    /// <inheritdoc />
    public partial class AddedAccomodations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Accommodation",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "AccomodationEmail",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "AccomodationPhone",
                table: "Trips");

            migrationBuilder.AddColumn<int>(
                name: "AccomodationId",
                table: "Trips",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Accomodation",
                columns: table => new
                {
                    AccomodationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accomodation", x => x.AccomodationId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Trips_AccomodationId",
                table: "Trips",
                column: "AccomodationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_Accomodation_AccomodationId",
                table: "Trips",
                column: "AccomodationId",
                principalTable: "Accomodation",
                principalColumn: "AccomodationId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Trips_Accomodation_AccomodationId",
                table: "Trips");

            migrationBuilder.DropTable(
                name: "Accomodation");

            migrationBuilder.DropIndex(
                name: "IX_Trips_AccomodationId",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "AccomodationId",
                table: "Trips");

            migrationBuilder.AddColumn<string>(
                name: "Accommodation",
                table: "Trips",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AccomodationEmail",
                table: "Trips",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AccomodationPhone",
                table: "Trips",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
