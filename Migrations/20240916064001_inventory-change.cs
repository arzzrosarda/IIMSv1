using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IIMSv1.Migrations
{
    /// <inheritdoc />
    public partial class inventorychange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemSpecs");

            migrationBuilder.AddColumn<string>(
                name: "ItemSpecsTypeId",
                table: "Items",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "itemEstPrice",
                table: "ItemPrice",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.CreateIndex(
                name: "IX_Items_ItemSpecsTypeId",
                table: "Items",
                column: "ItemSpecsTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_ItemSpecType_ItemSpecsTypeId",
                table: "Items",
                column: "ItemSpecsTypeId",
                principalTable: "ItemSpecType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_ItemSpecType_ItemSpecsTypeId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_ItemSpecsTypeId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "ItemSpecsTypeId",
                table: "Items");

            migrationBuilder.AlterColumn<double>(
                name: "itemEstPrice",
                table: "ItemPrice",
                type: "float",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "ItemSpecs",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ItemId = table.Column<string>(type: "nvarchar(450)", nullable: false),
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
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
    }
}
