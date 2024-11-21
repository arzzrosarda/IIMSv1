using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IIMSv1.Migrations
{
    /// <inheritdoc />
    public partial class ItemQuantity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_QuantityHistory_ItemQuantityId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_ItemQuantityId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "ItemQuantityId",
                table: "Items");

            migrationBuilder.AddColumn<int>(
                name: "ItemQuantity",
                table: "Items",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ItemQuantity",
                table: "Items");

            migrationBuilder.AddColumn<string>(
                name: "ItemQuantityId",
                table: "Items",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Items_ItemQuantityId",
                table: "Items",
                column: "ItemQuantityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_QuantityHistory_ItemQuantityId",
                table: "Items",
                column: "ItemQuantityId",
                principalTable: "QuantityHistory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
