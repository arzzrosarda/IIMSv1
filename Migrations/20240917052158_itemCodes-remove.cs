using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IIMSv1.Migrations
{
    /// <inheritdoc />
    public partial class itemCodesremove : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "supplyCode",
                table: "Supplies");

            migrationBuilder.DropColumn(
                name: "codeName",
                table: "ItemType");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "supplyCode",
                table: "Supplies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "codeName",
                table: "ItemType",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
