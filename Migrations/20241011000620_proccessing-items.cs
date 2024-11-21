using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IIMSv1.Migrations
{
    /// <inheritdoc />
    public partial class proccessingitems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "processings",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ItemId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DepartmentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    itemQuantity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    itemStatus = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_processings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_processings_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_processings_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_processings_DepartmentId",
                table: "processings",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_processings_ItemId",
                table: "processings",
                column: "ItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "processings");
        }
    }
}
