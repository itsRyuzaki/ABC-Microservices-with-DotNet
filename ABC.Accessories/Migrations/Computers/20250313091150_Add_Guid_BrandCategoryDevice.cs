using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ABC.Accessories.Migrations.Computers
{
    /// <inheritdoc />
    public partial class Add_Guid_BrandCategoryDevice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Guid",
                schema: "abc-computers",
                table: "DeviceModel",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Guid",
                schema: "abc-computers",
                table: "Category",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Guid",
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
                name: "Guid",
                schema: "abc-computers",
                table: "DeviceModel");

            migrationBuilder.DropColumn(
                name: "Guid",
                schema: "abc-computers",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "Guid",
                schema: "abc-computers",
                table: "Brands");
        }
    }
}
