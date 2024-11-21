using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IIMSv1.Migrations
{
    /// <inheritdoc />
    public partial class transferhistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_item_status_StatusId",
                table: "Items");

            migrationBuilder.DropIndex(
                name: "IX_Items_StatusId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "itemStatus",
                table: "processings");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "Items");

            migrationBuilder.AddColumn<string>(
                name: "StatusId",
                table: "processings",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "TransferHistories",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProcessingId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Activity = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransferHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransferHistories_processings_ProcessingId",
                        column: x => x.ProcessingId,
                        principalTable: "processings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_processings_StatusId",
                table: "processings",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferHistories_ProcessingId",
                table: "TransferHistories",
                column: "ProcessingId");

            migrationBuilder.AddForeignKey(
                name: "FK_processings_item_status_StatusId",
                table: "processings",
                column: "StatusId",
                principalTable: "item_status",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_processings_item_status_StatusId",
                table: "processings");

            migrationBuilder.DropTable(
                name: "TransferHistories");

            migrationBuilder.DropIndex(
                name: "IX_processings_StatusId",
                table: "processings");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "processings");

            migrationBuilder.AddColumn<string>(
                name: "itemStatus",
                table: "processings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

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
    }
}
