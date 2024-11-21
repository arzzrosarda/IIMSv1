using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IIMSv1.Migrations
{
    /// <inheritdoc />
    public partial class DBInventoryChange3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_ItemType_ItemTypeId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_ItemTypeId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "ItemTypeId",
                table: "Items");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ItemTypeId",
                table: "Items",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Items_ItemTypeId",
                table: "Items",
                column: "ItemTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_ItemType_ItemTypeId",
                table: "Items",
                column: "ItemTypeId",
                principalTable: "ItemType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
