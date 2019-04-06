using Microsoft.EntityFrameworkCore.Migrations;

namespace PortfolioInsight.Migrations
{
    public partial class Symbols_ListingExchangeCode_Optional : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Symbols_Name_ListingExchangeCode",
                table: "Symbols");

            migrationBuilder.BeforeSymbolsListingExchangeCodeChange();

            migrationBuilder.AlterColumn<string>(
                name: "ListingExchangeCode",
                table: "Symbols",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 10);

            migrationBuilder.SetSymbolsListingExchangeCodeNullCollationToCaseSensitive();
            migrationBuilder.AfterSymbolsListingExchangeCodeChange();

            migrationBuilder.CreateIndex(
                name: "IX_Symbols_Name_ListingExchangeCode",
                table: "Symbols",
                columns: new[] { "Name", "ListingExchangeCode" },
                unique: true,
                filter: "[ListingExchangeCode] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Symbols_Name_ListingExchangeCode",
                table: "Symbols");

            migrationBuilder.BeforeSymbolsListingExchangeCodeChange();

            migrationBuilder.AlterColumn<string>(
                name: "ListingExchangeCode",
                table: "Symbols",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.SetSymbolsListingExchangeCodeNotNullCollationToCaseSensitive();
            migrationBuilder.AfterSymbolsListingExchangeCodeChange();

            migrationBuilder.CreateIndex(
                name: "IX_Symbols_Name_ListingExchangeCode",
                table: "Symbols",
                columns: new[] { "Name", "ListingExchangeCode" },
                unique: true);
        }
    }
}
