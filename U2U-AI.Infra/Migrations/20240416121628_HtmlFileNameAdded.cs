using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace U2U_AI.Infra.Migrations
{
    /// <inheritdoc />
    public partial class HtmlFileNameAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HtmlFileName",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HtmlFileName",
                table: "Courses");
        }
    }
}
