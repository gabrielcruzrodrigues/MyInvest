using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyInvestAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserAndPurseEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Purses_Users_User_Id1",
                table: "Purses");

            migrationBuilder.DropIndex(
                name: "IX_Purses_User_Id1",
                table: "Purses");

            migrationBuilder.DropColumn(
                name: "User_Id1",
                table: "Purses");

            migrationBuilder.CreateIndex(
                name: "IX_Purses_User_Id",
                table: "Purses",
                column: "User_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Purses_Users_User_Id",
                table: "Purses",
                column: "User_Id",
                principalTable: "Users",
                principalColumn: "User_Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Purses_Users_User_Id",
                table: "Purses");

            migrationBuilder.DropIndex(
                name: "IX_Purses_User_Id",
                table: "Purses");

            migrationBuilder.AddColumn<int>(
                name: "User_Id1",
                table: "Purses",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Purses_User_Id1",
                table: "Purses",
                column: "User_Id1");

            migrationBuilder.AddForeignKey(
                name: "FK_Purses_Users_User_Id1",
                table: "Purses",
                column: "User_Id1",
                principalTable: "Users",
                principalColumn: "User_Id");
        }
    }
}
