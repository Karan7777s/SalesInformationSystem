using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SalesInformationSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class addedNewFieldsinProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Product",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProductDescriptiion",
                table: "Product",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "ProductDescriptiion",
                table: "Product");
        }
    }
}
