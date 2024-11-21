using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IIMSv1.Migrations
{
    /// <inheritdoc />
    public partial class adddatetimereleased : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransferHistories_processings_ProcessingId",
                table: "TransferHistories");

            migrationBuilder.DropTable(
                name: "processings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_item_status",
                table: "item_status");

            migrationBuilder.RenameTable(
                name: "item_status",
                newName: "Status");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Status",
                table: "Status",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Released",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ItemId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DepartmentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ReleaseQuantity = table.Column<int>(type: "int", nullable: false),
                    StatusId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DateReleased = table.Column<DateOnly>(type: "date", nullable: false),
                    TimeReleased = table.Column<TimeOnly>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Released", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Released_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Released_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Released_Status_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Status",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Released_DepartmentId",
                table: "Released",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Released_ItemId",
                table: "Released",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Released_StatusId",
                table: "Released",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_TransferHistories_Released_ProcessingId",
                table: "TransferHistories",
                column: "ProcessingId",
                principalTable: "Released",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransferHistories_Released_ProcessingId",
                table: "TransferHistories");

            migrationBuilder.DropTable(
                name: "Released");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Status",
                table: "Status");

            migrationBuilder.RenameTable(
                name: "Status",
                newName: "item_status");

            migrationBuilder.AddPrimaryKey(
                name: "PK_item_status",
                table: "item_status",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "processings",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DepartmentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ItemId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StatusId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ReleaseQuantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_processings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_processings_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_processings_Items_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_processings_item_status_StatusId",
                        column: x => x.StatusId,
                        principalTable: "item_status",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_processings_DepartmentId",
                table: "processings",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_processings_ItemId",
                table: "processings",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_processings_StatusId",
                table: "processings",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_TransferHistories_processings_ProcessingId",
                table: "TransferHistories",
                column: "ProcessingId",
                principalTable: "processings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
