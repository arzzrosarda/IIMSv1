using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IIMSv1.Migrations
{
    /// <inheritdoc />
    public partial class itemStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StatusId",
                table: "Items",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Items_StatusId",
                table: "Items",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_item_status_StatusId",
                table: "Items",
                column: "StatusId",
                principalTable: "item_status",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_item_status_StatusId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_StatusId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "Items");
        }
    }
}
