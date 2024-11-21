using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IIMSv1.Migrations
{
    /// <inheritdoc />
    public partial class PriceitemId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_ItemPrice_ItemPriceId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_ItemPriceId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "ItemPriceId",
                table: "Items");

            migrationBuilder.AddColumn<string>(
                name: "itemId",
                table: "ItemPrice",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ItemPrice_itemId",
                table: "ItemPrice",
                column: "itemId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemPrice_Items_itemId",
                table: "ItemPrice",
                column: "itemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemPrice_Items_itemId",
                table: "ItemPrice");

            migrationBuilder.DropIndex(
                name: "IX_ItemPrice_itemId",
                table: "ItemPrice");

            migrationBuilder.DropColumn(
                name: "itemId",
                table: "ItemPrice");

            migrationBuilder.AddColumn<string>(
                name: "ItemPriceId",
                table: "Items",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Items_ItemPriceId",
                table: "Items",
                column: "ItemPriceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_ItemPrice_ItemPriceId",
                table: "Items",
                column: "ItemPriceId",
                principalTable: "ItemPrice",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
