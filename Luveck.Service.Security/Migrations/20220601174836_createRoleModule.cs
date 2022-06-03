using Microsoft.EntityFrameworkCore.Migrations;

namespace Luveck.Service.Security.Migrations
{
    public partial class createRoleModule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_RoleModules",
                table: "RoleModules");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "RoleModules");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "RoleModules",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoleModules",
                table: "RoleModules",
                column: "Id");
        }
    }
}
