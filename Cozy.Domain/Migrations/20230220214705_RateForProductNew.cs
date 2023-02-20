using Microsoft.EntityFrameworkCore.Migrations;

namespace Cozy.Domain.Migrations
{
    public partial class RateForProductNew : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookRates_Products_ProductId",
                table: "BookRates");

            migrationBuilder.DropForeignKey(
                name: "FK_BookRates_Users_CreatedByUserId",
                table: "BookRates");

            migrationBuilder.DropForeignKey(
                name: "FK_BookRates_Users_DeletedByUserId",
                table: "BookRates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookRates",
                table: "BookRates");

            migrationBuilder.RenameTable(
                name: "BookRates",
                newName: "ProductRates");

            migrationBuilder.RenameIndex(
                name: "IX_BookRates_DeletedByUserId",
                table: "ProductRates",
                newName: "IX_ProductRates_DeletedByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_BookRates_CreatedByUserId",
                table: "ProductRates",
                newName: "IX_ProductRates_CreatedByUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductRates",
                table: "ProductRates",
                columns: new[] { "ProductId", "CreatedByUserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ProductRates_Products_ProductId",
                table: "ProductRates",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductRates_Users_CreatedByUserId",
                table: "ProductRates",
                column: "CreatedByUserId",
                principalSchema: "Membership",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductRates_Users_DeletedByUserId",
                table: "ProductRates",
                column: "DeletedByUserId",
                principalSchema: "Membership",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductRates_Products_ProductId",
                table: "ProductRates");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductRates_Users_CreatedByUserId",
                table: "ProductRates");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductRates_Users_DeletedByUserId",
                table: "ProductRates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductRates",
                table: "ProductRates");

            migrationBuilder.RenameTable(
                name: "ProductRates",
                newName: "BookRates");

            migrationBuilder.RenameIndex(
                name: "IX_ProductRates_DeletedByUserId",
                table: "BookRates",
                newName: "IX_BookRates_DeletedByUserId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductRates_CreatedByUserId",
                table: "BookRates",
                newName: "IX_BookRates_CreatedByUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookRates",
                table: "BookRates",
                columns: new[] { "ProductId", "CreatedByUserId" });

            migrationBuilder.AddForeignKey(
                name: "FK_BookRates_Products_ProductId",
                table: "BookRates",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookRates_Users_CreatedByUserId",
                table: "BookRates",
                column: "CreatedByUserId",
                principalSchema: "Membership",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookRates_Users_DeletedByUserId",
                table: "BookRates",
                column: "DeletedByUserId",
                principalSchema: "Membership",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
