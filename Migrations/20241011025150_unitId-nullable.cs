using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IIMSv1.Migrations
{
    /// <inheritdoc />
    public partial class unitIdnullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_ItemUnits_ItemUnitId",
                table: "Items");

            migrationBuilder.AlterColumn<string>(
                name: "ItemUnitId",
                table: "Items",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_ItemUnits_ItemUnitId",
                table: "Items",
                column: "ItemUnitId",
                principalTable: "ItemUnits",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_ItemUnits_ItemUnitId",
                table: "Items");

            migrationBuilder.AlterColumn<string>(
                name: "ItemUnitId",
                table: "Items",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_ItemUnits_ItemUnitId",
                table: "Items",
                column: "ItemUnitId",
                principalTable: "ItemUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
