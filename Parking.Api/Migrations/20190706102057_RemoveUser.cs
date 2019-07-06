using Microsoft.EntityFrameworkCore.Migrations;

namespace Parking.Api.Migrations
{
    public partial class RemoveUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ParkingPlaces_Users_UserId",
                table: "ParkingPlaces");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropIndex(
                name: "IX_ParkingPlaces_UserId",
                table: "ParkingPlaces");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "ParkingPlaces",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "ParkingPlaces",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ParkingPlaces_UserId",
                table: "ParkingPlaces",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ParkingPlaces_Users_UserId",
                table: "ParkingPlaces",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
