using Microsoft.EntityFrameworkCore.Migrations;

namespace PortfolioInsight.Migrations
{
    public partial class Fix_USD_Currency : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "USA");

            migrationBuilder.InsertData(
                table: "Currencies",
                column: "Code",
                value: "USD");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "USD");

            migrationBuilder.InsertData(
                table: "Currencies",
                column: "Code",
                value: "USA");
        }
    }
}
