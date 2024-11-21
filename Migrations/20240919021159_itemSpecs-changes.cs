using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IIMSv1.Migrations
{
    /// <inheritdoc />
    public partial class itemSpecschanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_ItemSpecs_ItemSpecsId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_ItemSpecsId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "ItemSpecsId",
                table: "Items");

            migrationBuilder.AddColumn<string>(
                name: "itemId",
                table: "ItemSpecs",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ItemSpecs_itemId",
                table: "ItemSpecs",
                column: "itemId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemSpecs_Items_itemId",
                table: "ItemSpecs",
                column: "itemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemSpecs_Items_itemId",
                table: "ItemSpecs");

            migrationBuilder.DropIndex(
                name: "IX_ItemSpecs_itemId",
                table: "ItemSpecs");

            migrationBuilder.DropColumn(
                name: "itemId",
                table: "ItemSpecs");

            migrationBuilder.AddColumn<string>(
                name: "ItemSpecsId",
                table: "Items",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Items_ItemSpecsId",
                table: "Items",
                column: "ItemSpecsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_ItemSpecs_ItemSpecsId",
                table: "Items",
                column: "ItemSpecsId",
                principalTable: "ItemSpecs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
