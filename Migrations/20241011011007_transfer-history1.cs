using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IIMSv1.Migrations
{
    /// <inheritdoc />
    public partial class transferhistory1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Activity",
                table: "TransferHistories",
                newName: "ActivityTo");

            migrationBuilder.AddColumn<string>(
                name: "ActivityFrom",
                table: "TransferHistories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "ActivityFrom",
                table: "TransferHistories");

            migrationBuilder.DropColumn(
                name: "Dates",
                table: "TransferHistories");

            migrationBuilder.RenameColumn(
                name: "ActivityTo",
                table: "TransferHistories",
                newName: "Activity");
        }
    }
}
