using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class Migration2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FloorID",
                table: "ParkingSpaces",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ParkingSpaces_FloorID",
                table: "ParkingSpaces",
                column: "FloorID");

            migrationBuilder.AddForeignKey(
                name: "FK_ParkingSpaces_Floors_FloorID",
                table: "ParkingSpaces",
                column: "FloorID",
                principalTable: "Floors",
                principalColumn: "FloorID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ParkingSpaces_Floors_FloorID",
                table: "ParkingSpaces");

            migrationBuilder.DropIndex(
                name: "IX_ParkingSpaces_FloorID",
                table: "ParkingSpaces");

            migrationBuilder.DropColumn(
                name: "FloorID",
                table: "ParkingSpaces");
        }
    }
}
