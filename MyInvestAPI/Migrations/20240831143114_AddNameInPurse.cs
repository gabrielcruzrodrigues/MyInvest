using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyInvestAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddNameInPurse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Purses",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Purses");
        }
    }
}
