using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IIMSv1.Migrations
{
    /// <inheritdoc />
    public partial class Add_Department1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "departmentID",
                table: "Employees",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_departmentID",
                table: "Employees",
                column: "departmentID");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Departments_departmentID",
                table: "Employees",
                column: "departmentID",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Departments_departmentID",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_departmentID",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "departmentID",
                table: "Employees");
        }
    }
}
