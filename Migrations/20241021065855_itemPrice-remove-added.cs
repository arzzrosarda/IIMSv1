using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IIMSv1.Migrations
{
    /// <inheritdoc />
    public partial class itemPriceremoveadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemPrice_ItemUnits_itemUnitId",
                table: "ItemPrice");

            migrationBuilder.DropForeignKey(
                name: "FK_processings_ItemPrice_ItemId",
                table: "processings");

            migrationBuilder.DropIndex(
                name: "IX_ItemPrice_itemUnitId",
                table: "ItemPrice");

            migrationBuilder.DropColumn(
                name: "ItemQuantity",
                table: "ItemPrice");

            migrationBuilder.DropColumn(
                name: "itemUnitId",
                table: "ItemPrice");

            migrationBuilder.RenameColumn(
                name: "itemQuantity",
                table: "processings",
                newName: "ReleaseQuantity");

            migrationBuilder.AddColumn<int>(
                name: "itemQuantity",
                table: "Items",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "itemUnitId",
                table: "Items",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_processings_Items_ItemId",
                table: "processings",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_processings_Items_ItemId",
                table: "processings");

            migrationBuilder.DropColumn(
                name: "itemQuantity",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "itemUnitId",
                table: "Items");

            migrationBuilder.RenameColumn(
                name: "ReleaseQuantity",
                table: "processings",
                newName: "itemQuantity");

            migrationBuilder.AddColumn<int>(
                name: "ItemQuantity",
                table: "ItemPrice",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "itemUnitId",
                table: "ItemPrice",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ItemPrice_itemUnitId",
                table: "ItemPrice",
                column: "itemUnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemPrice_ItemUnits_itemUnitId",
                table: "ItemPrice",
                column: "itemUnitId",
                principalTable: "ItemUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_processings_ItemPrice_ItemId",
                table: "processings",
                column: "ItemId",
                principalTable: "ItemPrice",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
