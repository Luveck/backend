using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Luveck.Service.Administration.Migrations
{
    public partial class updateCountry : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Iso",
                table: "Country");

            migrationBuilder.DropColumn(
                name: "StateCode",
                table: "City");

            migrationBuilder.RenameColumn(
                name: "StateCode",
                table: "Department",
                newName: "UpdateBy");

            migrationBuilder.AddColumn<string>(
                name: "CreateBy",
                table: "Department",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "Department",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                table: "Department",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "status",
                table: "Department",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateBy",
                table: "Department");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "Department");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                table: "Department");

            migrationBuilder.DropColumn(
                name: "status",
                table: "Department");

            migrationBuilder.RenameColumn(
                name: "UpdateBy",
                table: "Department",
                newName: "StateCode");

            migrationBuilder.AddColumn<string>(
                name: "Iso",
                table: "Country",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StateCode",
                table: "City",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
