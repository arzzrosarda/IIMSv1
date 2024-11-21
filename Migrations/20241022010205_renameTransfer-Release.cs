using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IIMSv1.Migrations
{
    /// <inheritdoc />
    public partial class renameTransferRelease : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TransferHistories");

            migrationBuilder.CreateTable(
                name: "ReleaseHistory",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    Time = table.Column<TimeOnly>(type: "time", nullable: false),
                    ReleaseId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    OldQuantity = table.Column<int>(type: "int", nullable: false),
                    NewQuantity = table.Column<int>(type: "int", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReleaseHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReleaseHistory_Released_ReleaseId",
                        column: x => x.ReleaseId,
                        principalTable: "Released",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReleaseHistory_ReleaseId",
                table: "ReleaseHistory",
                column: "ReleaseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReleaseHistory");

            migrationBuilder.CreateTable(
                name: "TransferHistories",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProcessingId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    NewQuantity = table.Column<int>(type: "int", nullable: false),
                    OldQuantity = table.Column<int>(type: "int", nullable: false),
                    Time = table.Column<TimeOnly>(type: "time", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransferHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransferHistories_Released_ProcessingId",
                        column: x => x.ProcessingId,
                        principalTable: "Released",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TransferHistories_ProcessingId",
                table: "TransferHistories",
                column: "ProcessingId");
        }
    }
}
