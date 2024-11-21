using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IIMSv1.Migrations
{
    /// <inheritdoc />
    public partial class Inventorytbls4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemSpecType_Departments_departmentId",
                table: "ItemSpecType");

            migrationBuilder.DropIndex(
                name: "IX_ItemSpecType_departmentId",
                table: "ItemSpecType");

            migrationBuilder.DropColumn(
                name: "departmentId",
                table: "ItemSpecType");

            migrationBuilder.AlterColumn<string>(
                name: "ItemId",
                table: "ItemSpecs",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_ItemSpecs_ItemId",
                table: "ItemSpecs",
                column: "ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemSpecs_Items_ItemId",
                table: "ItemSpecs",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemSpecs_Items_ItemId",
                table: "ItemSpecs");

            migrationBuilder.DropIndex(
                name: "IX_ItemSpecs_ItemId",
                table: "ItemSpecs");

            migrationBuilder.AddColumn<string>(
                name: "departmentId",
                table: "ItemSpecType",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "ItemId",
                table: "ItemSpecs",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_ItemSpecType_departmentId",
                table: "ItemSpecType",
                column: "departmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemSpecType_Departments_departmentId",
                table: "ItemSpecType",
                column: "departmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
