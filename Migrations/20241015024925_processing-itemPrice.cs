using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IIMSv1.Migrations
{
    /// <inheritdoc />
    public partial class processingitemPrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_processings_Items_ItemId",
                table: "processings");

            migrationBuilder.AddForeignKey(
                name: "FK_processings_ItemPrice_ItemId",
                table: "processings",
                column: "ItemId",
                principalTable: "ItemPrice",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_processings_ItemPrice_ItemId",
                table: "processings");

            migrationBuilder.AddForeignKey(
                name: "FK_processings_Items_ItemId",
                table: "processings",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
