using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyInvestAPI.Migrations
{
    /// <inheritdoc />
    public partial class changeActiveByEnable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Active",
                table: "Purses",
                newName: "Enable");

            migrationBuilder.RenameColumn(
                name: "Active",
                table: "AspNetUsers",
                newName: "Enable");

            migrationBuilder.AddColumn<int>(
                name: "Enable",
                table: "Actives",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Enable",
                table: "Actives");

            migrationBuilder.RenameColumn(
                name: "Enable",
                table: "Purses",
                newName: "Active");

            migrationBuilder.RenameColumn(
                name: "Enable",
                table: "AspNetUsers",
                newName: "Active");
        }
    }
}
