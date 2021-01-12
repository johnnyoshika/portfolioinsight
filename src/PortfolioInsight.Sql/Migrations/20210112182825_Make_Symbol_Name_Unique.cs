using Microsoft.EntityFrameworkCore.Migrations;

namespace PortfolioInsight.Migrations
{
    public partial class Make_Symbol_Name_Unique : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Symbols_Name_ListingExchangeCode",
                table: "Symbols");

            migrationBuilder.CreateIndex(
                name: "IX_Symbols_Name",
                table: "Symbols",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Symbols_Name",
                table: "Symbols");

            migrationBuilder.CreateIndex(
                name: "IX_Symbols_Name_ListingExchangeCode",
                table: "Symbols",
                columns: new[] { "Name", "ListingExchangeCode" },
                unique: true,
                filter: "[ListingExchangeCode] IS NOT NULL");
        }
    }
}
