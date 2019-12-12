using Microsoft.EntityFrameworkCore.Migrations;

namespace PortfolioInsight.Migrations
{
    public partial class Create_ExcludeAccounts_And_ExcludeSymbols : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExcludeAccounts",
                columns: table => new
                {
                    PortfolioId = table.Column<int>(nullable: false),
                    AccountId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExcludeAccounts", x => new { x.PortfolioId, x.AccountId });
                    table.ForeignKey(
                        name: "FK_ExcludeAccounts_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExcludeAccounts_Portfolios_PortfolioId",
                        column: x => x.PortfolioId,
                        principalTable: "Portfolios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExcludeSymbols",
                columns: table => new
                {
                    PortfolioId = table.Column<int>(nullable: false),
                    AccountId = table.Column<int>(nullable: false),
                    SymbolId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExcludeSymbols", x => new { x.PortfolioId, x.AccountId, x.SymbolId });
                    table.ForeignKey(
                        name: "FK_ExcludeSymbols_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExcludeSymbols_Portfolios_PortfolioId",
                        column: x => x.PortfolioId,
                        principalTable: "Portfolios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExcludeSymbols_Symbols_SymbolId",
                        column: x => x.SymbolId,
                        principalTable: "Symbols",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExcludeAccounts_AccountId",
                table: "ExcludeAccounts",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_ExcludeSymbols_AccountId",
                table: "ExcludeSymbols",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_ExcludeSymbols_SymbolId",
                table: "ExcludeSymbols",
                column: "SymbolId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExcludeAccounts");

            migrationBuilder.DropTable(
                name: "ExcludeSymbols");
        }
    }
}
