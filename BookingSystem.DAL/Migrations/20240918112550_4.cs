using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class _4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Floors_FloorID",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Offices_OfficeID",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Workspaces_WorkspaceID",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_ParkingSpaces_Floors_FloorID",
                table: "ParkingSpaces");

            migrationBuilder.DropForeignKey(
                name: "FK_ParkingSpaces_Offices_OfficeID",
                table: "ParkingSpaces");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Departments_DepartmentID",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Workspaces_Offices_OfficeID",
                table: "Workspaces");

            migrationBuilder.DropIndex(
                name: "IX_Workspaces_OfficeID",
                table: "Workspaces");

            migrationBuilder.DropIndex(
                name: "IX_ParkingSpaces_OfficeID",
                table: "ParkingSpaces");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_FloorID",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_OfficeID",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "OfficeID",
                table: "Workspaces");

            migrationBuilder.DropColumn(
                name: "OfficeID",
                table: "ParkingSpaces");

            migrationBuilder.DropColumn(
                name: "FloorID",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "OfficeID",
                table: "Bookings");

            migrationBuilder.RenameColumn(
                name: "MimeType",
                table: "Floors",
                newName: "ImageMimeType");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Bookings",
                newName: "BookingDate");

            migrationBuilder.AlterColumn<int>(
                name: "WorkspaceID",
                table: "Bookings",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "AdditionalRequirements",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Workspaces_WorkspaceID",
                table: "Bookings",
                column: "WorkspaceID",
                principalTable: "Workspaces",
                principalColumn: "WorkspaceID");

            migrationBuilder.AddForeignKey(
                name: "FK_ParkingSpaces_Floors_FloorID",
                table: "ParkingSpaces",
                column: "FloorID",
                principalTable: "Floors",
                principalColumn: "FloorID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Departments_DepartmentID",
                table: "Users",
                column: "DepartmentID",
                principalTable: "Departments",
                principalColumn: "DepartmentID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Workspaces_WorkspaceID",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_ParkingSpaces_Floors_FloorID",
                table: "ParkingSpaces");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Departments_DepartmentID",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "ImageMimeType",
                table: "Floors",
                newName: "MimeType");

            migrationBuilder.RenameColumn(
                name: "BookingDate",
                table: "Bookings",
                newName: "Date");

            migrationBuilder.AddColumn<int>(
                name: "OfficeID",
                table: "Workspaces",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OfficeID",
                table: "ParkingSpaces",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "WorkspaceID",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AdditionalRequirements",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FloorID",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OfficeID",
                table: "Bookings",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Workspaces_OfficeID",
                table: "Workspaces",
                column: "OfficeID");

            migrationBuilder.CreateIndex(
                name: "IX_ParkingSpaces_OfficeID",
                table: "ParkingSpaces",
                column: "OfficeID");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_FloorID",
                table: "Bookings",
                column: "FloorID");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_OfficeID",
                table: "Bookings",
                column: "OfficeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Floors_FloorID",
                table: "Bookings",
                column: "FloorID",
                principalTable: "Floors",
                principalColumn: "FloorID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Offices_OfficeID",
                table: "Bookings",
                column: "OfficeID",
                principalTable: "Offices",
                principalColumn: "OfficeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Workspaces_WorkspaceID",
                table: "Bookings",
                column: "WorkspaceID",
                principalTable: "Workspaces",
                principalColumn: "WorkspaceID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ParkingSpaces_Floors_FloorID",
                table: "ParkingSpaces",
                column: "FloorID",
                principalTable: "Floors",
                principalColumn: "FloorID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ParkingSpaces_Offices_OfficeID",
                table: "ParkingSpaces",
                column: "OfficeID",
                principalTable: "Offices",
                principalColumn: "OfficeID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Departments_DepartmentID",
                table: "Users",
                column: "DepartmentID",
                principalTable: "Departments",
                principalColumn: "DepartmentID");

            migrationBuilder.AddForeignKey(
                name: "FK_Workspaces_Offices_OfficeID",
                table: "Workspaces",
                column: "OfficeID",
                principalTable: "Offices",
                principalColumn: "OfficeID");
        }
    }
}
