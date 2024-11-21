using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IIMSv1.Migrations
{
    /// <inheritdoc />
    public partial class Inventorytbls3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Supplies_Departments_departmentId",
                table: "Supplies");

            migrationBuilder.DropIndex(
                name: "IX_Supplies_departmentId",
                table: "Supplies");

            migrationBuilder.DropColumn(
                name: "departmentId",
                table: "Supplies");

            migrationBuilder.AlterColumn<string>(
                name: "ItemTypeId",
                table: "Items",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_ItemType_ItemTypeId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_ItemTypeId",
                table: "Items");

            migrationBuilder.AddColumn<string>(
                name: "departmentId",
                table: "Supplies",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "ItemTypeId",
                table: "Items",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_Supplies_departmentId",
                table: "Supplies",
                column: "departmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Supplies_Departments_departmentId",
                table: "Supplies",
                column: "departmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
