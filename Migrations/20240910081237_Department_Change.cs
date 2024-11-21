using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IIMSv1.Migrations
{
    /// <inheritdoc />
    public partial class Department_Change : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departments_departmentClusters_Department",
                table: "Departments");

            migrationBuilder.DropIndex(
                name: "IX_Departments_Department",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "Department",
                table: "Departments");

            migrationBuilder.AlterColumn<string>(
                name: "DepartmentClusterID",
                table: "Departments",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_DepartmentClusterID",
                table: "Departments",
                column: "DepartmentClusterID");

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_departmentClusters_DepartmentClusterID",
                table: "Departments",
                column: "DepartmentClusterID",
                principalTable: "departmentClusters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departments_departmentClusters_DepartmentClusterID",
                table: "Departments");

            migrationBuilder.DropIndex(
                name: "IX_Departments_DepartmentClusterID",
                table: "Departments");

            migrationBuilder.AlterColumn<string>(
                name: "DepartmentClusterID",
                table: "Departments",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "Department",
                table: "Departments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Departments_Department",
                table: "Departments",
                column: "Department");

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_departmentClusters_Department",
                table: "Departments",
                column: "Department",
                principalTable: "departmentClusters",
                principalColumn: "Id");
        }
    }
}
