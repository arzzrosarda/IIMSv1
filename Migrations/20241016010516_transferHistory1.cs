using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IIMSv1.Migrations
{
    /// <inheritdoc />
    public partial class transferHistory1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransferHistories_ItemDateHistories_Dates",
                table: "TransferHistories");

            migrationBuilder.DropTable(
                name: "ItemDateHistories");

            migrationBuilder.DropIndex(
                name: "IX_TransferHistories_Dates",
                table: "TransferHistories");

            migrationBuilder.DropColumn(
                name: "Dates",
                table: "TransferHistories");

            migrationBuilder.RenameColumn(
                name: "ActivityTo",
                table: "TransferHistories",
                newName: "OldQuantity");

            migrationBuilder.RenameColumn(
                name: "ActivityFrom",
                table: "TransferHistories",
                newName: "NewQuantity");

            migrationBuilder.AddColumn<string>(
                name: "Action",
                table: "TransferHistories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Action",
                table: "TransferHistories");

            migrationBuilder.RenameColumn(
                name: "OldQuantity",
                table: "TransferHistories",
                newName: "ActivityTo");

            migrationBuilder.RenameColumn(
                name: "NewQuantity",
                table: "TransferHistories",
                newName: "ActivityFrom");

            migrationBuilder.AddColumn<string>(
                name: "Dates",
                table: "TransferHistories",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "ItemDateHistories",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Dates = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemDateHistories", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TransferHistories_Dates",
                table: "TransferHistories",
                column: "Dates");

            migrationBuilder.AddForeignKey(
                name: "FK_TransferHistories_ItemDateHistories_Dates",
                table: "TransferHistories",
                column: "Dates",
                principalTable: "ItemDateHistories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
