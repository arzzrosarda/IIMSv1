using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IIMSv1.Migrations
{
    /// <inheritdoc />
    public partial class DBInventoryChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_Departments_departmetId",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_ItemPrice_ItemPriceId",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Items_ItemSpecType_ItemSpecsTypeId",
                table: "Items");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemSpecType_ItemSpecValue_ItemSpecValueId",
                table: "ItemSpecType");

            migrationBuilder.DropIndex(
                name: "IX_ItemSpecType_ItemSpecValueId",
                table: "ItemSpecType");

            migrationBuilder.DropIndex(
                name: "IX_Items_departmetId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_ItemPriceId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_ItemSpecsTypeId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "ItemSpecValueId",
                table: "ItemSpecType");

            migrationBuilder.DropColumn(
                name: "ItemPriceId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "ItemSpecsTypeId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "departmetId",
                table: "Items");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "ItemType",
                newName: "itemType");

            migrationBuilder.AddColumn<string>(
                name: "departmetId",
                table: "Supplies",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ItemTypeId",
                table: "ItemSpecValue",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SpecTypeId",
                table: "ItemSpecValue",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "itemId",
                table: "ItemPrice",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Supplies_departmetId",
                table: "Supplies",
                column: "departmetId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemSpecValue_ItemTypeId",
                table: "ItemSpecValue",
                column: "ItemTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemSpecValue_SpecTypeId",
                table: "ItemSpecValue",
                column: "SpecTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemPrice_itemId",
                table: "ItemPrice",
                column: "itemId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemPrice_Items_itemId",
                table: "ItemPrice",
                column: "itemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemSpecValue_ItemSpecType_SpecTypeId",
                table: "ItemSpecValue",
                column: "SpecTypeId",
                principalTable: "ItemSpecType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemSpecValue_ItemType_ItemTypeId",
                table: "ItemSpecValue",
                column: "ItemTypeId",
                principalTable: "ItemType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Supplies_Departments_departmetId",
                table: "Supplies",
                column: "departmetId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemPrice_Items_itemId",
                table: "ItemPrice");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemSpecValue_ItemSpecType_SpecTypeId",
                table: "ItemSpecValue");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemSpecValue_ItemType_ItemTypeId",
                table: "ItemSpecValue");

            migrationBuilder.DropForeignKey(
                name: "FK_Supplies_Departments_departmetId",
                table: "Supplies");

            migrationBuilder.DropIndex(
                name: "IX_Supplies_departmetId",
                table: "Supplies");

            migrationBuilder.DropIndex(
                name: "IX_ItemSpecValue_ItemTypeId",
                table: "ItemSpecValue");

            migrationBuilder.DropIndex(
                name: "IX_ItemSpecValue_SpecTypeId",
                table: "ItemSpecValue");

            migrationBuilder.DropIndex(
                name: "IX_ItemPrice_itemId",
                table: "ItemPrice");

            migrationBuilder.DropColumn(
                name: "departmetId",
                table: "Supplies");

            migrationBuilder.DropColumn(
                name: "ItemTypeId",
                table: "ItemSpecValue");

            migrationBuilder.DropColumn(
                name: "SpecTypeId",
                table: "ItemSpecValue");

            migrationBuilder.DropColumn(
                name: "itemId",
                table: "ItemPrice");

            migrationBuilder.RenameColumn(
                name: "itemType",
                table: "ItemType",
                newName: "Name");

            migrationBuilder.AddColumn<string>(
                name: "ItemSpecValueId",
                table: "ItemSpecType",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ItemPriceId",
                table: "Items",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ItemSpecsTypeId",
                table: "Items",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "departmetId",
                table: "Items",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ItemSpecType_ItemSpecValueId",
                table: "ItemSpecType",
                column: "ItemSpecValueId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_departmetId",
                table: "Items",
                column: "departmetId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_ItemPriceId",
                table: "Items",
                column: "ItemPriceId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_ItemSpecsTypeId",
                table: "Items",
                column: "ItemSpecsTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_Departments_departmetId",
                table: "Items",
                column: "departmetId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_ItemPrice_ItemPriceId",
                table: "Items",
                column: "ItemPriceId",
                principalTable: "ItemPrice",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Items_ItemSpecType_ItemSpecsTypeId",
                table: "Items",
                column: "ItemSpecsTypeId",
                principalTable: "ItemSpecType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ItemSpecType_ItemSpecValue_ItemSpecValueId",
                table: "ItemSpecType",
                column: "ItemSpecValueId",
                principalTable: "ItemSpecValue",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
