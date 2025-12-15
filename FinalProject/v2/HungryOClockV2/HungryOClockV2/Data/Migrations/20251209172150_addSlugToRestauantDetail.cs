using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HungryOClockV2.Data.Migrations
{
    /// <inheritdoc />
    public partial class addSlugToRestauantDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Slug",
                table: "Restaurants",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Slug",
                table: "Restaurants");
        }
    }
}
