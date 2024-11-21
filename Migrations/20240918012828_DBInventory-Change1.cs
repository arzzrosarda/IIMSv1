using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IIMSv1.Migrations
{
    /// <inheritdoc />
    public partial class DBInventoryChange1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ItemSpecs",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ItemId = table.Column<string>(type: "nvarchar(450)", nullable: true),
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
                        name: "FK_ItemSpecs_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ItemSpecs_ItemId",
                table: "ItemSpecs",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemSpecs_SpecValueId",
                table: "ItemSpecs",
                column: "SpecValueId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemSpecs");
        }
    }
}
