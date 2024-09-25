using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class MigrationPositionXY : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Position",
                table: "Workspaces",
                newName: "PositionY");

            migrationBuilder.RenameColumn(
                name: "Position",
                table: "ParkingSpaces",
                newName: "PositionY");

            migrationBuilder.AddColumn<string>(
                name: "PositionX",
                table: "Workspaces",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PositionX",
                table: "ParkingSpaces",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PositionX",
                table: "Workspaces");

            migrationBuilder.DropColumn(
                name: "PositionX",
                table: "ParkingSpaces");

            migrationBuilder.RenameColumn(
                name: "PositionY",
                table: "Workspaces",
                newName: "Position");

            migrationBuilder.RenameColumn(
                name: "PositionY",
                table: "ParkingSpaces",
                newName: "Position");
        }
    }
}
