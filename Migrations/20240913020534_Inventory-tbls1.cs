using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IIMSv1.Migrations
{
    /// <inheritdoc />
    public partial class Inventorytbls1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Supplies_AspNetUsers_UserId",
                table: "Supplies");

            migrationBuilder.AddForeignKey(
                name: "FK_Supplies_Employees_UserId",
                table: "Supplies",
                column: "UserId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Supplies_Employees_UserId",
                table: "Supplies");

            migrationBuilder.AddForeignKey(
                name: "FK_Supplies_AspNetUsers_UserId",
                table: "Supplies",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
