using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace U2U_AI.Infra.Migrations
{
    /// <inheritdoc />
    public partial class InitialSessionData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Session",
                columns: new[] { "Id", "CourseId", "OrganisationDays" },
                values: new object[,]
                {
                    { 1, "building-aspnet-web-apis", "[\"2024-05-13\",\"2024-05-14\",\"2024-05-15\",\"2024-05-16\",\"2024-05-17\"]" },
                    { 2, "building-aspnet-web-apis", "[\"2024-05-20\",\"2024-05-21\",\"2024-05-22\",\"2024-05-23\",\"2024-05-24\"]" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Session",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Session",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
