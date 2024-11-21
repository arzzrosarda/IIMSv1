using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IIMSv1.Migrations
{
    /// <inheritdoc />
    public partial class RemoveDeptIds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Supplies_Departments_departmetId",
                table: "Supplies");

            migrationBuilder.DropIndex(
                name: "IX_Supplies_departmetId",
                table: "Supplies");

            migrationBuilder.DropColumn(
                name: "departmetId",
                table: "Supplies");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "ItemUnits");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "ItemSpecType");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "departmetId",
                table: "Supplies",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DepartmentId",
                table: "ItemUnits",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DepartmentId",
                table: "ItemSpecType",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Supplies_departmetId",
                table: "Supplies",
                column: "departmetId");

            migrationBuilder.AddForeignKey(
                name: "FK_Supplies_Departments_departmetId",
                table: "Supplies",
                column: "departmetId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
