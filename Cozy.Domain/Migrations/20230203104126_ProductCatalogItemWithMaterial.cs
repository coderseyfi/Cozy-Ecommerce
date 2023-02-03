using Microsoft.EntityFrameworkCore.Migrations;

namespace Cozy.Domain.Migrations
{
    public partial class ProductCatalogItemWithMaterial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaterialId",
                table: "ProductCatalogItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ProductCatalogItems_MaterialId",
                table: "ProductCatalogItems",
                column: "MaterialId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductCatalogItems_Materials_MaterialId",
                table: "ProductCatalogItems",
                column: "MaterialId",
                principalTable: "Materials",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductCatalogItems_Materials_MaterialId",
                table: "ProductCatalogItems");

            migrationBuilder.DropIndex(
                name: "IX_ProductCatalogItems_MaterialId",
                table: "ProductCatalogItems");

            migrationBuilder.DropColumn(
                name: "MaterialId",
                table: "ProductCatalogItems");
        }
    }
}
