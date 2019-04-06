using Microsoft.EntityFrameworkCore.Migrations;

namespace PortfolioInsight.Migrations
{
    public partial class Create_ListingExchanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ListingExchangeCode",
                table: "Symbols",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "ListingExchanges",
                columns: table => new
                {
                    Code = table.Column<string>(maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListingExchanges", x => x.Code);
                });

            migrationBuilder.InsertData(
                table: "ListingExchanges",
                column: "Code",
                values: new object[]
                {
                    "TSX",
                    "TSXV",
                    "CNSX",
                    "MX",
                    "NASDAQ",
                    "NYSE",
                    "NYSEAM",
                    "ARCA",
                    "OPRA",
                    "PinkSheets",
                    "OTCBB"
                });

            migrationBuilder.CreateIndex(
                name: "IX_Symbols_ListingExchangeCode",
                table: "Symbols",
                column: "ListingExchangeCode");

            migrationBuilder.CreateIndex(
                name: "IX_Symbols_Name_ListingExchangeCode",
                table: "Symbols",
                columns: new[] { "Name", "ListingExchangeCode" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Symbols_ListingExchanges_ListingExchangeCode",
                table: "Symbols",
                column: "ListingExchangeCode",
                principalTable: "ListingExchanges",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Symbols_ListingExchanges_ListingExchangeCode",
                table: "Symbols");

            migrationBuilder.DropTable(
                name: "ListingExchanges");

            migrationBuilder.DropIndex(
                name: "IX_Symbols_ListingExchangeCode",
                table: "Symbols");

            migrationBuilder.DropIndex(
                name: "IX_Symbols_Name_ListingExchangeCode",
                table: "Symbols");

            migrationBuilder.DropColumn(
                name: "ListingExchangeCode",
                table: "Symbols");
        }
    }
}
