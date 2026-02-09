using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ABC.Accessories.Migrations.Computers
{
    /// <inheritdoc />
    public partial class Add_OgPrice_Update_Type : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "OriginalPrice",
                schema: "abc-computers",
                table: "Accessories",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OriginalPrice",
                schema: "abc-computers",
                table: "Accessories");
        }
    }
}
