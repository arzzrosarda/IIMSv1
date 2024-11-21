using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IIMSv1.Migrations
{
    /// <inheritdoc />
    public partial class Inventorytbls2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Supplies_Employees_UserId",
                table: "Supplies");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Supplies",
                newName: "departmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Supplies_UserId",
                table: "Supplies",
                newName: "IX_Supplies_departmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Supplies_Departments_departmentId",
                table: "Supplies",
                column: "departmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Supplies_Departments_departmentId",
                table: "Supplies");

            migrationBuilder.RenameColumn(
                name: "departmentId",
                table: "Supplies",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Supplies_departmentId",
                table: "Supplies",
                newName: "IX_Supplies_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Supplies_Employees_UserId",
                table: "Supplies",
                column: "UserId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
