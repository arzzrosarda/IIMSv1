using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IIMSv1.Migrations
{
    /// <inheritdoc />
    public partial class removestatustbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Released_Status_StatusId",
                table: "Released");

            migrationBuilder.DropTable(
                name: "Status");

            migrationBuilder.DropIndex(
                name: "IX_Released_StatusId",
                table: "Released");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "Released");

            migrationBuilder.AddColumn<bool>(
                name: "Pullout",
                table: "Released",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Received",
                table: "Released",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ReleaseNumber",
                table: "Released",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Pullout",
                table: "Released");

            migrationBuilder.DropColumn(
                name: "Received",
                table: "Released");

            migrationBuilder.DropColumn(
                name: "ReleaseNumber",
                table: "Released");

            migrationBuilder.AddColumn<string>(
                name: "StatusId",
                table: "Released",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Status",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Status", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Released_StatusId",
                table: "Released",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Released_Status_StatusId",
                table: "Released",
                column: "StatusId",
                principalTable: "Status",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
