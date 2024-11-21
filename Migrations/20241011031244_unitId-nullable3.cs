using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IIMSv1.Migrations
{
    /// <inheritdoc />
    public partial class unitIdnullable3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "itemUnitId",
                table: "ItemPrice",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemPrice_ItemUnits_itemUnitId",
                table: "ItemPrice");

            migrationBuilder.DropIndex(
                name: "IX_ItemPrice_itemUnitId",
                table: "ItemPrice");

            migrationBuilder.AlterColumn<string>(
                name: "itemUnitId",
                table: "ItemPrice",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
