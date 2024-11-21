using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IIMSv1.Migrations
{
    /// <inheritdoc />
    public partial class ItemQuantityHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ItemId",
                table: "QuantityHistory",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_QuantityHistory_ItemId",
                table: "QuantityHistory",
                column: "ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_QuantityHistory_Items_ItemId",
                table: "QuantityHistory",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuantityHistory_Items_ItemId",
                table: "QuantityHistory");

            migrationBuilder.DropIndex(
                name: "IX_QuantityHistory_ItemId",
                table: "QuantityHistory");

            migrationBuilder.DropColumn(
                name: "ItemId",
                table: "QuantityHistory");
        }
    }
}
