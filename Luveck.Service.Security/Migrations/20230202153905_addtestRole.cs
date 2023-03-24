using Microsoft.EntityFrameworkCore.Migrations;

namespace Luveck.Service.Security.Migrations
{
    public partial class addtestRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2a00befd-c908-46ab-9f64-149b9654d01b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4278a0b7-a4bf-4f07-aa2f-ce6812ec70ce");

            migrationBuilder.AddColumn<string>(
                name: "test",
                table: "AspNetRoles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Discriminator", "Name", "NormalizedName" },
                values: new object[] { "c4e1602d-f42c-4d45-8e8d-f01967f76f24", "0", "IdentityRole", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Discriminator", "Name", "NormalizedName" },
                values: new object[] { "88d46e48-8076-4ec6-8160-6c346ca12ec8", "1", "IdentityRole", "Cliente", "CLIENTE" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "88d46e48-8076-4ec6-8160-6c346ca12ec8");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c4e1602d-f42c-4d45-8e8d-f01967f76f24");

            migrationBuilder.DropColumn(
                name: "test",
                table: "AspNetRoles");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Discriminator", "Name", "NormalizedName" },
                values: new object[] { "4278a0b7-a4bf-4f07-aa2f-ce6812ec70ce", "0", "IdentityRole", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Discriminator", "Name", "NormalizedName" },
                values: new object[] { "2a00befd-c908-46ab-9f64-149b9654d01b", "1", "IdentityRole", "Cliente", "CLIENTE" });
        }
    }
}
