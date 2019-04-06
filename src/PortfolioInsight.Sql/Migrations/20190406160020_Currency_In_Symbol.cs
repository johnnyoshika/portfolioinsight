using Microsoft.EntityFrameworkCore.Migrations;

namespace PortfolioInsight.Migrations
{
    public partial class Currency_In_Symbol : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Positions_Currencies_CurrencyCode",
                table: "Positions");

            migrationBuilder.DropIndex(
                name: "IX_Positions_CurrencyCode",
                table: "Positions");

            migrationBuilder.DropColumn(
                name: "CurrencyCode",
                table: "Positions");

            migrationBuilder.AddColumn<string>(
                name: "CurrencyCode",
                table: "Symbols",
                maxLength: 3,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ListingExchange",
                table: "Symbols",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.SetSymbolsCurrencyCodeCollationToCaseSensitive();

            migrationBuilder.CreateIndex(
                name: "IX_Symbols_CurrencyCode",
                table: "Symbols",
                column: "CurrencyCode");

            migrationBuilder.AddForeignKey(
                name: "FK_Symbols_Currencies_CurrencyCode",
                table: "Symbols",
                column: "CurrencyCode",
                principalTable: "Currencies",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Symbols_Currencies_CurrencyCode",
                table: "Symbols");

            migrationBuilder.DropIndex(
                name: "IX_Symbols_CurrencyCode",
                table: "Symbols");

            migrationBuilder.DropColumn(
                name: "CurrencyCode",
                table: "Symbols");

            migrationBuilder.DropColumn(
                name: "ListingExchange",
                table: "Symbols");

            migrationBuilder.AddColumn<string>(
                name: "CurrencyCode",
                table: "Positions",
                maxLength: 3,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Positions_CurrencyCode",
                table: "Positions",
                column: "CurrencyCode");

            migrationBuilder.AddForeignKey(
                name: "FK_Positions_Currencies_CurrencyCode",
                table: "Positions",
                column: "CurrencyCode",
                principalTable: "Currencies",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
