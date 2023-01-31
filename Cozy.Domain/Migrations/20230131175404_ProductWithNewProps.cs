using Microsoft.EntityFrameworkCore.Migrations;

namespace Cozy.Domain.Migrations
{
    public partial class ProductWithNewProps : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsInStock",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Rate",
                table: "Products");

            migrationBuilder.AddColumn<string>(
                name: "ShortDescription",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShortDescription",
                table: "Products");

            migrationBuilder.AddColumn<bool>(
                name: "IsInStock",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "Rate",
                table: "Products",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
