using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IIMSv1.Migrations
{
    /// <inheritdoc />
    public partial class DBInventoryChange4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemSpecs_Items_ItemId",
                table: "ItemSpecs");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemSpecValue_ItemType_ItemTypeId",
                table: "ItemSpecValue");

            migrationBuilder.DropIndex(
                name: "IX_ItemSpecValue_ItemTypeId",
                table: "ItemSpecValue");

            migrationBuilder.DropIndex(
                name: "IX_ItemSpecs_ItemId",
                table: "ItemSpecs");

            migrationBuilder.DropColumn(
                name: "ItemTypeId",
                table: "ItemSpecValue");

            migrationBuilder.DropColumn(
                name: "ItemId",
                table: "ItemSpecs");

            migrationBuilder.AddColumn<string>(
                name: "ItemSpecsId",
                table: "Items",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ItemTypeId",
                table: "Items",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Items_ItemSpecsId",
                table: "Items",
                column: "ItemSpecsId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_ItemTypeId",
                table: "Items",
                column: "ItemTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_ItemSpecs_ItemSpecsId",
                table: "Items",
                column: "ItemSpecsId",
                principalTable: "ItemSpecs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_ItemType_ItemTypeId",
                table: "Items",
                column: "ItemTypeId",
                principalTable: "ItemType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_ItemSpecs_ItemSpecsId",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_ItemType_ItemTypeId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_ItemSpecsId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_ItemTypeId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "ItemSpecsId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "ItemTypeId",
                table: "Items");

            migrationBuilder.AddColumn<string>(
                name: "ItemTypeId",
                table: "ItemSpecValue",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ItemId",
                table: "ItemSpecs",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ItemSpecValue_ItemTypeId",
                table: "ItemSpecValue",
                column: "ItemTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemSpecs_ItemId",
                table: "ItemSpecs",
                column: "ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemSpecs_Items_ItemId",
                table: "ItemSpecs",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemSpecValue_ItemType_ItemTypeId",
                table: "ItemSpecValue",
                column: "ItemTypeId",
                principalTable: "ItemType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
