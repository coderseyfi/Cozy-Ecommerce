using Microsoft.EntityFrameworkCore.Migrations;

namespace Cozy.Domain.Migrations
{
    public partial class ProductCatalogItemsNew : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductCatalogItems",
                table: "ProductCatalogItems");

            migrationBuilder.DropIndex(
                name: "IX_ProductCatalogItems_ProductId",
                table: "ProductCatalogItems");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductCatalogItems",
                table: "ProductCatalogItems",
                columns: new[] { "ProductId", "ColorId", "MaterialId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductCatalogItems",
                table: "ProductCatalogItems");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductCatalogItems",
                table: "ProductCatalogItems",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCatalogItems_ProductId",
                table: "ProductCatalogItems",
                column: "ProductId");
        }
    }
}
