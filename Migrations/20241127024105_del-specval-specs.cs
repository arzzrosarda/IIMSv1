using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IIMSv1.Migrations
{
    /// <inheritdoc />
    public partial class delspecvalspecs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemSpecs");

            migrationBuilder.DropTable(
                name: "ItemSpecValue");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ItemSpecValue",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SpecTypeId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    itemSpecValue = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemSpecValue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemSpecValue_ItemSpecType_SpecTypeId",
                        column: x => x.SpecTypeId,
                        principalTable: "ItemSpecType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemSpecs",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    itemId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SpecValueId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemSpecs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemSpecs_ItemSpecValue_SpecValueId",
                        column: x => x.SpecValueId,
                        principalTable: "ItemSpecValue",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemSpecs_Items_itemId",
                        column: x => x.itemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ItemSpecs_itemId",
                table: "ItemSpecs",
                column: "itemId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemSpecs_SpecValueId",
                table: "ItemSpecs",
                column: "SpecValueId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemSpecValue_SpecTypeId",
                table: "ItemSpecValue",
                column: "SpecTypeId");
        }
    }
}
