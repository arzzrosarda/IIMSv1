using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IIMSv1.Migrations
{
    /// <inheritdoc />
    public partial class Add_Department : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateTable(
                name: "departmentClusters",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NormalizedName = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_departmentClusters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NormalizedName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DepartmentClusterID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Department = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Departments_departmentClusters_Department",
                        column: x => x.Department,
                        principalTable: "departmentClusters",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_UserID",
                table: "AspNetUsers",
                column: "UserID",
                unique: true,
                filter: "[UserID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_departmentClusters_NormalizedName",
                table: "departmentClusters",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Departments_Department",
                table: "Departments",
                column: "Department");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Employees_UserID",
                table: "AspNetUsers",
                column: "UserID",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Employees_UserID",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "departmentClusters");

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
    }
}
