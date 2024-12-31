using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyInvestAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddOperationPropertiesMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Operation",
                table: "SmtpProperties",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Operation",
                table: "SmtpProperties");
        }
    }
}
