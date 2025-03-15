using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ABC.Accessories.Migrations.Computers
{
    /// <inheritdoc />
    public partial class Add_Brand_Images : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AltText",
                schema: "abc-computers",
                table: "Brands",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Order",
                schema: "abc-computers",
                table: "Brands",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Source",
                schema: "abc-computers",
                table: "Brands",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AltText",
                schema: "abc-computers",
                table: "Brands");

            migrationBuilder.DropColumn(
                name: "Order",
                schema: "abc-computers",
                table: "Brands");

            migrationBuilder.DropColumn(
                name: "Source",
                schema: "abc-computers",
                table: "Brands");
        }
    }
}
