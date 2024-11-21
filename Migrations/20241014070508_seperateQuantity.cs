using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IIMSv1.Migrations
{
    /// <inheritdoc />
    public partial class seperateQuantity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "ItemQuantity",
                table: "Items");

            migrationBuilder.AlterColumn<int>(
                name: "QuantityTo",
                table: "QuantityHistory",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "QuantityFrom",
                table: "QuantityHistory",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "ItemQuantityId",
                table: "Items",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Items_ItemQuantityId",
                table: "Items",
                column: "ItemQuantityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_QuantityHistory_ItemQuantityId",
                table: "Items",
                column: "ItemQuantityId",
                principalTable: "QuantityHistory",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<string>(
                name: "QuantityTo",
                table: "QuantityHistory",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "QuantityFrom",
                table: "QuantityHistory",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "ItemId",
                table: "QuantityHistory",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "ItemQuantity",
                table: "Items",
                type: "int",
                nullable: false,
                defaultValue: 0);

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
    }
}
