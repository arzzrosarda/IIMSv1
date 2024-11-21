using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IIMSv1.Migrations
{
    /// <inheritdoc />
    public partial class Employee2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Employees_UserID",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_UserID",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "AspNetUsers",
                newName: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_EmployeeID",
                table: "AspNetUsers",
                column: "EmployeeID",
                unique: true,
                filter: "[EmployeeID] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Employees_EmployeeID",
                table: "AspNetUsers",
                column: "EmployeeID",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Employees_EmployeeID",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_EmployeeID",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "EmployeeID",
                table: "AspNetUsers",
                newName: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_UserID",
                table: "AspNetUsers",
                column: "UserID",
                unique: true,
                filter: "[UserID] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Employees_UserID",
                table: "AspNetUsers",
                column: "UserID",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
