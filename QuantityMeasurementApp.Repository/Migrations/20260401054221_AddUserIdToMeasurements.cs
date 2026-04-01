using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuantityMeasurementApp.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdToMeasurements : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Measurements",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Measurements");
        }
    }
}
