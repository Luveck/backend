using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Luveck.Service.Administration.Migrations
{
    public partial class creationInformation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_City_Department_departmentId",
                table: "City");

            migrationBuilder.DropForeignKey(
                name: "FK_Department_Country_countryId",
                table: "Department");

            migrationBuilder.DropForeignKey(
                name: "FK_Medical_Patology_patologyId",
                table: "Medical");

            migrationBuilder.DropForeignKey(
                name: "FK_Pharmacy_City_cityId",
                table: "Pharmacy");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_Category_categoryId",
                table: "Product");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductChangeRule_Product_productId",
                table: "ProductChangeRule");

            migrationBuilder.DropForeignKey(
                name: "FK_Purchase_Pharmacy_pharmacyId",
                table: "Purchase");

            migrationBuilder.AlterColumn<int>(
                name: "pharmacyId",
                table: "Purchase",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "purchaseReviewed",
                table: "Purchase",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "userId",
                table: "Purchase",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "productId",
                table: "ProductChangeRule",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "categoryId",
                table: "Product",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "cityId",
                table: "Pharmacy",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "patologyId",
                table: "Medical",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "countryId",
                table: "Department",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "departmentId",
                table: "City",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "ExchangedProducts",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    QuantityExchanged = table.Column<int>(type: "int", nullable: false),
                    userExchanged = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    productId = table.Column<int>(type: "int", nullable: false),
                    ExchangeBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExchangeDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExchangedProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExchangedProducts_Product_productId",
                        column: x => x.productId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductPurchases",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    purchaseId = table.Column<int>(type: "int", nullable: false),
                    productId = table.Column<int>(type: "int", nullable: false),
                    QuantityShiped = table.Column<int>(type: "int", nullable: false),
                    DateShiped = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Exchanged = table.Column<bool>(type: "bit", nullable: false),
                    Losed = table.Column<bool>(type: "bit", nullable: false),
                    CreateBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductPurchases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductPurchases_Product_productId",
                        column: x => x.productId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductPurchases_Purchase_purchaseId",
                        column: x => x.purchaseId,
                        principalTable: "Purchase",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExchangedProducts_productId",
                table: "ExchangedProducts",
                column: "productId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductPurchases_productId",
                table: "ProductPurchases",
                column: "productId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductPurchases_purchaseId",
                table: "ProductPurchases",
                column: "purchaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_City_Department_departmentId",
                table: "City",
                column: "departmentId",
                principalTable: "Department",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Department_Country_countryId",
                table: "Department",
                column: "countryId",
                principalTable: "Country",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Medical_Patology_patologyId",
                table: "Medical",
                column: "patologyId",
                principalTable: "Patology",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pharmacy_City_cityId",
                table: "Pharmacy",
                column: "cityId",
                principalTable: "City",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Category_categoryId",
                table: "Product",
                column: "categoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductChangeRule_Product_productId",
                table: "ProductChangeRule",
                column: "productId",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Purchase_Pharmacy_pharmacyId",
                table: "Purchase",
                column: "pharmacyId",
                principalTable: "Pharmacy",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_City_Department_departmentId",
                table: "City");

            migrationBuilder.DropForeignKey(
                name: "FK_Department_Country_countryId",
                table: "Department");

            migrationBuilder.DropForeignKey(
                name: "FK_Medical_Patology_patologyId",
                table: "Medical");

            migrationBuilder.DropForeignKey(
                name: "FK_Pharmacy_City_cityId",
                table: "Pharmacy");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_Category_categoryId",
                table: "Product");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductChangeRule_Product_productId",
                table: "ProductChangeRule");

            migrationBuilder.DropForeignKey(
                name: "FK_Purchase_Pharmacy_pharmacyId",
                table: "Purchase");

            migrationBuilder.DropTable(
                name: "ExchangedProducts");

            migrationBuilder.DropTable(
                name: "ProductPurchases");

            migrationBuilder.DropColumn(
                name: "purchaseReviewed",
                table: "Purchase");

            migrationBuilder.DropColumn(
                name: "userId",
                table: "Purchase");

            migrationBuilder.AlterColumn<int>(
                name: "pharmacyId",
                table: "Purchase",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "productId",
                table: "ProductChangeRule",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "categoryId",
                table: "Product",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "cityId",
                table: "Pharmacy",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "patologyId",
                table: "Medical",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "countryId",
                table: "Department",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "departmentId",
                table: "City",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_City_Department_departmentId",
                table: "City",
                column: "departmentId",
                principalTable: "Department",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Department_Country_countryId",
                table: "Department",
                column: "countryId",
                principalTable: "Country",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Medical_Patology_patologyId",
                table: "Medical",
                column: "patologyId",
                principalTable: "Patology",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Pharmacy_City_cityId",
                table: "Pharmacy",
                column: "cityId",
                principalTable: "City",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Category_categoryId",
                table: "Product",
                column: "categoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductChangeRule_Product_productId",
                table: "ProductChangeRule",
                column: "productId",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Purchase_Pharmacy_pharmacyId",
                table: "Purchase",
                column: "pharmacyId",
                principalTable: "Pharmacy",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
