using Microsoft.EntityFrameworkCore.Migrations;

namespace Luveck.Service.Security.Migrations
{
    public partial class removeColumnRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoleModules_modules_ModuleId",
                table: "RoleModules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_modules",
                table: "modules");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "avilableDeleteTipe",
                table: "AspNetRoles");

            migrationBuilder.RenameTable(
                name: "modules",
                newName: "Modules");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Modules",
                table: "Modules",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RoleModules_Modules_ModuleId",
                table: "RoleModules",
                column: "ModuleId",
                principalTable: "Modules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoleModules_Modules_ModuleId",
                table: "RoleModules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Modules",
                table: "Modules");

            migrationBuilder.RenameTable(
                name: "Modules",
                newName: "modules");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetRoles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "avilableDeleteTipe",
                table: "AspNetRoles",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_modules",
                table: "modules",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RoleModules_modules_ModuleId",
                table: "RoleModules",
                column: "ModuleId",
                principalTable: "modules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
