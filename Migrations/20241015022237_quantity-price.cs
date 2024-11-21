using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IIMSv1.Migrations
{
    /// <inheritdoc />
    public partial class quantityprice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ItemQuantity",
                table: "Items");

            migrationBuilder.AddColumn<int>(
                name: "ItemQuantity",
                table: "ItemPrice",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ItemQuantity",
                table: "ItemPrice");

            migrationBuilder.AddColumn<int>(
                name: "ItemQuantity",
                table: "Items",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
