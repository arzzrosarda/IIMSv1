using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IIMSv1.Migrations
{
    /// <inheritdoc />
    public partial class Inventorytbls : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ItemPrice",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    itemEstPrice = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Year = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemPrice", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ItemSpecType",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SpecType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    departmentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemSpecType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemSpecType_Departments_departmentId",
                        column: x => x.departmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemUnits",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UnitName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemUnits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Supplies",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    supplyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    supplyCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Supplies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Supplies_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemSpecValue",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SpecTypeId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SpecValue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false)
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
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    departmetId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ItemCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ItemName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ItemTypeId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ItemUnitId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ItemPriceId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Items_Departments_departmetId",
                        column: x => x.departmetId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Items_ItemPrice_ItemPriceId",
                        column: x => x.ItemPriceId,
                        principalTable: "ItemPrice",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Items_ItemUnits_ItemUnitId",
                        column: x => x.ItemUnitId,
                        principalTable: "ItemUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemType",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    codeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SuppliesId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemType_Supplies_SuppliesId",
                        column: x => x.SuppliesId,
                        principalTable: "Supplies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemSpecs",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ItemId = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                });

            migrationBuilder.CreateIndex(
                name: "IX_Items_departmetId",
                table: "Items",
                column: "departmetId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_ItemPriceId",
                table: "Items",
                column: "ItemPriceId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_ItemUnitId",
                table: "Items",
                column: "ItemUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemSpecs_SpecValueId",
                table: "ItemSpecs",
                column: "SpecValueId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemSpecType_departmentId",
                table: "ItemSpecType",
                column: "departmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemSpecValue_SpecTypeId",
                table: "ItemSpecValue",
                column: "SpecTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemType_SuppliesId",
                table: "ItemType",
                column: "SuppliesId");

            migrationBuilder.CreateIndex(
                name: "IX_Supplies_UserId",
                table: "Supplies",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "ItemSpecs");

            migrationBuilder.DropTable(
                name: "ItemType");

            migrationBuilder.DropTable(
                name: "ItemPrice");

            migrationBuilder.DropTable(
                name: "ItemUnits");

            migrationBuilder.DropTable(
                name: "ItemSpecValue");

            migrationBuilder.DropTable(
                name: "Supplies");

            migrationBuilder.DropTable(
                name: "ItemSpecType");
        }
    }
}
