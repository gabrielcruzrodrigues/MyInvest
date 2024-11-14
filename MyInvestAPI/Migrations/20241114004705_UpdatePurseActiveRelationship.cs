using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyInvestAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePurseActiveRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivePurse");

            migrationBuilder.AddColumn<int>(
                name: "PurseId",
                table: "Actives",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Actives_PurseId",
                table: "Actives",
                column: "PurseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Actives_Purses_PurseId",
                table: "Actives",
                column: "PurseId",
                principalTable: "Purses",
                principalColumn: "Purse_Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Actives_Purses_PurseId",
                table: "Actives");

            migrationBuilder.DropIndex(
                name: "IX_Actives_PurseId",
                table: "Actives");

            migrationBuilder.DropColumn(
                name: "PurseId",
                table: "Actives");

            migrationBuilder.CreateTable(
                name: "ActivePurse",
                columns: table => new
                {
                    ActivesActive_Id = table.Column<int>(type: "integer", nullable: false),
                    PursesPurse_Id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivePurse", x => new { x.ActivesActive_Id, x.PursesPurse_Id });
                    table.ForeignKey(
                        name: "FK_ActivePurse_Actives_ActivesActive_Id",
                        column: x => x.ActivesActive_Id,
                        principalTable: "Actives",
                        principalColumn: "Active_Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActivePurse_Purses_PursesPurse_Id",
                        column: x => x.PursesPurse_Id,
                        principalTable: "Purses",
                        principalColumn: "Purse_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActivePurse_PursesPurse_Id",
                table: "ActivePurse",
                column: "PursesPurse_Id");
        }
    }
}
