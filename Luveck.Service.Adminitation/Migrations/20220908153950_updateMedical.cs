using Microsoft.EntityFrameworkCore.Migrations;

namespace Luveck.Service.Administration.Migrations
{
    public partial class updateMedical : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Medical_Patology_categoryId",
                table: "Medical");

            migrationBuilder.RenameColumn(
                name: "categoryId",
                table: "Medical",
                newName: "patologyId");

            migrationBuilder.RenameIndex(
                name: "IX_Medical_categoryId",
                table: "Medical",
                newName: "IX_Medical_patologyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Medical_Patology_patologyId",
                table: "Medical",
                column: "patologyId",
                principalTable: "Patology",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Medical_Patology_patologyId",
                table: "Medical");

            migrationBuilder.RenameColumn(
                name: "patologyId",
                table: "Medical",
                newName: "categoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Medical_patologyId",
                table: "Medical",
                newName: "IX_Medical_categoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Medical_Patology_categoryId",
                table: "Medical",
                column: "categoryId",
                principalTable: "Patology",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
