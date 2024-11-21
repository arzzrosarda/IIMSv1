using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IIMSv1.Migrations
{
    /// <inheritdoc />
    public partial class processingQuantity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_QuantityHistory_ItemQuantityId",
                table: "Items");

            migrationBuilder.AlterColumn<int>(
                name: "itemQuantity",
                table: "processings",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ItemQuantityId",
                table: "Items",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_QuantityHistory_ItemQuantityId",
                table: "Items",
                column: "ItemQuantityId",
                principalTable: "QuantityHistory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_QuantityHistory_ItemQuantityId",
                table: "Items");

            migrationBuilder.AlterColumn<string>(
                name: "itemQuantity",
                table: "processings",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "ItemQuantityId",
                table: "Items",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_QuantityHistory_ItemQuantityId",
                table: "Items",
                column: "ItemQuantityId",
                principalTable: "QuantityHistory",
                principalColumn: "Id");
        }
    }
}
