using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IIMSv1.Migrations
{
    /// <inheritdoc />
    public partial class inventorychange1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemSpecValue_ItemSpecType_SpecTypeId",
                table: "ItemSpecValue");

            migrationBuilder.DropIndex(
                name: "IX_ItemSpecValue_SpecTypeId",
                table: "ItemSpecValue");

            migrationBuilder.DropColumn(
                name: "SpecTypeId",
                table: "ItemSpecValue");

            migrationBuilder.RenameColumn(
                name: "SpecValue",
                table: "ItemSpecValue",
                newName: "itemSpecValue");

            migrationBuilder.RenameColumn(
                name: "SpecType",
                table: "ItemSpecType",
                newName: "itemSpecType");

            migrationBuilder.AddColumn<string>(
                name: "ItemSpecValueId",
                table: "ItemSpecType",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ItemSpecType_ItemSpecValueId",
                table: "ItemSpecType",
                column: "ItemSpecValueId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemSpecType_ItemSpecValue_ItemSpecValueId",
                table: "ItemSpecType",
                column: "ItemSpecValueId",
                principalTable: "ItemSpecValue",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemSpecType_ItemSpecValue_ItemSpecValueId",
                table: "ItemSpecType");

            migrationBuilder.DropIndex(
                name: "IX_ItemSpecType_ItemSpecValueId",
                table: "ItemSpecType");

            migrationBuilder.DropColumn(
                name: "ItemSpecValueId",
                table: "ItemSpecType");

            migrationBuilder.RenameColumn(
                name: "itemSpecValue",
                table: "ItemSpecValue",
                newName: "SpecValue");

            migrationBuilder.RenameColumn(
                name: "itemSpecType",
                table: "ItemSpecType",
                newName: "SpecType");

            migrationBuilder.AddColumn<string>(
                name: "SpecTypeId",
                table: "ItemSpecValue",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ItemSpecValue_SpecTypeId",
                table: "ItemSpecValue",
                column: "SpecTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemSpecValue_ItemSpecType_SpecTypeId",
                table: "ItemSpecValue",
                column: "SpecTypeId",
                principalTable: "ItemSpecType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
