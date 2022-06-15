using Microsoft.EntityFrameworkCore.Migrations;

namespace Luveck.Service.Security.Migrations
{
    public partial class updateRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "avilableDelet",
                table: "AspNetRoles");

            migrationBuilder.AddColumn<int>(
                name: "avilableDeleteTipe",
                table: "AspNetRoles",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "avilableDeleteTipe",
                table: "AspNetRoles");

            migrationBuilder.AddColumn<bool>(
                name: "avilableDelet",
                table: "AspNetRoles",
                type: "bit",
                nullable: true);
        }
    }
}
