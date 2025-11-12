using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TripsLog.Migrations
{
    /// <inheritdoc />
    public partial class AddActivitiesAndAccomodations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityTrip_Activity_ActivitiesActivityId",
                table: "ActivityTrip");

            migrationBuilder.DropForeignKey(
                name: "FK_Trips_Accomodation_AccomodationId",
                table: "Trips");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Activity",
                table: "Activity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Accomodation",
                table: "Accomodation");

            migrationBuilder.RenameTable(
                name: "Activity",
                newName: "Activities");

            migrationBuilder.RenameTable(
                name: "Accomodation",
                newName: "Accomodations");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Activities",
                table: "Activities",
                column: "ActivityId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Accomodations",
                table: "Accomodations",
                column: "AccomodationId");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityTrip_Activities_ActivitiesActivityId",
                table: "ActivityTrip",
                column: "ActivitiesActivityId",
                principalTable: "Activities",
                principalColumn: "ActivityId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_Accomodations_AccomodationId",
                table: "Trips",
                column: "AccomodationId",
                principalTable: "Accomodations",
                principalColumn: "AccomodationId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActivityTrip_Activities_ActivitiesActivityId",
                table: "ActivityTrip");

            migrationBuilder.DropForeignKey(
                name: "FK_Trips_Accomodations_AccomodationId",
                table: "Trips");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Activities",
                table: "Activities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Accomodations",
                table: "Accomodations");

            migrationBuilder.RenameTable(
                name: "Activities",
                newName: "Activity");

            migrationBuilder.RenameTable(
                name: "Accomodations",
                newName: "Accomodation");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Activity",
                table: "Activity",
                column: "ActivityId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Accomodation",
                table: "Accomodation",
                column: "AccomodationId");

            migrationBuilder.AddForeignKey(
                name: "FK_ActivityTrip_Activity_ActivitiesActivityId",
                table: "ActivityTrip",
                column: "ActivitiesActivityId",
                principalTable: "Activity",
                principalColumn: "ActivityId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_Accomodation_AccomodationId",
                table: "Trips",
                column: "AccomodationId",
                principalTable: "Accomodation",
                principalColumn: "AccomodationId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
