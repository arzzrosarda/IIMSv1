using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IIMSv1.Migrations
{
    /// <inheritdoc />
    public partial class unitEFItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "itemUnitId",
                table: "Items",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Items_itemUnitId",
                table: "Items",
                column: "itemUnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_ItemUnits_itemUnitId",
                table: "Items",
                column: "itemUnitId",
                principalTable: "ItemUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_ItemUnits_itemUnitId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_itemUnitId",
                table: "Items");

            migrationBuilder.AlterColumn<string>(
                name: "itemUnitId",
                table: "Items",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
