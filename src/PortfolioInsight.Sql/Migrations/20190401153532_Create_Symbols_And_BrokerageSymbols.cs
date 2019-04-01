using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PortfolioInsight.Migrations
{
    public partial class Create_Symbols_And_BrokerageSymbols : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Positions_Ticker_AccountId",
                table: "Positions");

            migrationBuilder.DropColumn(
                name: "Ticker",
                table: "Positions");

            migrationBuilder.AddColumn<int>(
                name: "SymbolId",
                table: "Positions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Symbols",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Symbols", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BrokerageSymbols",
                columns: table => new
                {
                    SymbolId = table.Column<int>(nullable: false),
                    BrokerageId = table.Column<int>(nullable: false),
                    ReferenceId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrokerageSymbols", x => new { x.SymbolId, x.BrokerageId });
                    table.ForeignKey(
                        name: "FK_BrokerageSymbols_Brokerages_BrokerageId",
                        column: x => x.BrokerageId,
                        principalTable: "Brokerages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BrokerageSymbols_Symbols_SymbolId",
                        column: x => x.SymbolId,
                        principalTable: "Symbols",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Positions_SymbolId_AccountId",
                table: "Positions",
                columns: new[] { "SymbolId", "AccountId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BrokerageSymbols_BrokerageId",
                table: "BrokerageSymbols",
                column: "BrokerageId");

            migrationBuilder.CreateIndex(
                name: "IX_BrokerageSymbols_ReferenceId_BrokerageId",
                table: "BrokerageSymbols",
                columns: new[] { "ReferenceId", "BrokerageId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Positions_Symbols_SymbolId",
                table: "Positions",
                column: "SymbolId",
                principalTable: "Symbols",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Positions_Symbols_SymbolId",
                table: "Positions");

            migrationBuilder.DropTable(
                name: "BrokerageSymbols");

            migrationBuilder.DropTable(
                name: "Symbols");

            migrationBuilder.DropIndex(
                name: "IX_Positions_SymbolId_AccountId",
                table: "Positions");

            migrationBuilder.DropColumn(
                name: "SymbolId",
                table: "Positions");

            migrationBuilder.AddColumn<string>(
                name: "Ticker",
                table: "Positions",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Positions_Ticker_AccountId",
                table: "Positions",
                columns: new[] { "Ticker", "AccountId" },
                unique: true);
        }
    }
}
