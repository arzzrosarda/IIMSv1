using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IIMSv1.Migrations
{
    /// <inheritdoc />
    public partial class unitIdnullable1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_ItemUnits_ItemUnitId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_ItemUnitId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "ItemUnitId",
                table: "Items");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ItemUnitId",
                table: "Items",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Items_ItemUnitId",
                table: "Items",
                column: "ItemUnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_ItemUnits_ItemUnitId",
                table: "Items",
                column: "ItemUnitId",
                principalTable: "ItemUnits",
                principalColumn: "Id");
        }
    }
}
