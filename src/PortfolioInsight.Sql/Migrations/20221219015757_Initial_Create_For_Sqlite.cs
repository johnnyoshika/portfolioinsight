using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PortfolioInsight.Migrations
{
    public partial class Initial_Create_For_Sqlite : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Brokerages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brokerages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    Code = table.Column<string>(type: "TEXT", maxLength: 3, nullable: false),
                    Rate = table.Column<decimal>(type: "decimal(14, 9)", nullable: false),
                    AsOf = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "ListingExchanges",
                columns: table => new
                {
                    Code = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListingExchanges", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(type: "TEXT", nullable: false),
                    LastName = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", maxLength: 48, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastActivityAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ActivityCount = table.Column<int>(type: "INTEGER", nullable: false),
                    LastLoginAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    LoginCount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Symbols",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 450, nullable: true),
                    CurrencyCode = table.Column<string>(type: "TEXT", maxLength: 3, nullable: false),
                    ListingExchangeCode = table.Column<string>(type: "TEXT", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Symbols", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Symbols_Currencies_CurrencyCode",
                        column: x => x.CurrencyCode,
                        principalTable: "Currencies",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Symbols_ListingExchanges_ListingExchangeCode",
                        column: x => x.ListingExchangeCode,
                        principalTable: "ListingExchanges",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Connections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BrokerageId = table.Column<int>(type: "INTEGER", nullable: false),
                    BrokerageUserId = table.Column<string>(type: "TEXT", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    RefreshToken = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Connections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Connections_Brokerages_BrokerageId",
                        column: x => x.BrokerageId,
                        principalTable: "Brokerages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Connections_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Portfolios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Portfolios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Portfolios_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BrokerageSymbols",
                columns: table => new
                {
                    SymbolId = table.Column<int>(type: "INTEGER", nullable: false),
                    BrokerageId = table.Column<int>(type: "INTEGER", nullable: false),
                    ReferenceId = table.Column<string>(type: "TEXT", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Number = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    ConnectionId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Accounts_Connections_ConnectionId",
                        column: x => x.ConnectionId,
                        principalTable: "Connections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Allocations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PortfolioId = table.Column<int>(type: "INTEGER", nullable: false),
                    SymbolId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Allocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Allocations_Portfolios_PortfolioId",
                        column: x => x.PortfolioId,
                        principalTable: "Portfolios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Allocations_Symbols_SymbolId",
                        column: x => x.SymbolId,
                        principalTable: "Symbols",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AssetClasses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Target = table.Column<decimal>(type: "decimal(4, 3)", nullable: true),
                    PortfolioId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetClasses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssetClasses_Portfolios_PortfolioId",
                        column: x => x.PortfolioId,
                        principalTable: "Portfolios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    PortfolioId = table.Column<int>(type: "INTEGER", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Json = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => new { x.PortfolioId, x.Date });
                    table.ForeignKey(
                        name: "FK_Reports_Portfolios_PortfolioId",
                        column: x => x.PortfolioId,
                        principalTable: "Portfolios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Balances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Value = table.Column<decimal>(type: "money", nullable: false),
                    Type = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    CurrencyCode = table.Column<string>(type: "TEXT", maxLength: 3, nullable: false),
                    AccountId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Balances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Balances_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Balances_Currencies_CurrencyCode",
                        column: x => x.CurrencyCode,
                        principalTable: "Currencies",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExcludeAccounts",
                columns: table => new
                {
                    PortfolioId = table.Column<int>(type: "INTEGER", nullable: false),
                    AccountId = table.Column<int>(type: "INTEGER", nullable: false)
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
                    PortfolioId = table.Column<int>(type: "INTEGER", nullable: false),
                    AccountId = table.Column<int>(type: "INTEGER", nullable: false),
                    SymbolId = table.Column<int>(type: "INTEGER", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "Positions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Value = table.Column<decimal>(type: "money", nullable: false),
                    SymbolId = table.Column<int>(type: "INTEGER", nullable: false),
                    AccountId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Positions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Positions_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Positions_Symbols_SymbolId",
                        column: x => x.SymbolId,
                        principalTable: "Symbols",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AllocationProportions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AllocationId = table.Column<int>(type: "INTEGER", nullable: false),
                    AssetClassId = table.Column<int>(type: "INTEGER", nullable: false),
                    Rate = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AllocationProportions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AllocationProportions_Allocations_AllocationId",
                        column: x => x.AllocationId,
                        principalTable: "Allocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AllocationProportions_AssetClasses_AssetClassId",
                        column: x => x.AssetClassId,
                        principalTable: "AssetClasses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Brokerages",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Questrade" });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Code", "AsOf", "Rate" },
                values: new object[] { "CAD", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Code", "AsOf", "Rate" },
                values: new object[] { "USD", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0m });

            migrationBuilder.InsertData(
                table: "ListingExchanges",
                column: "Code",
                value: "ARCA");

            migrationBuilder.InsertData(
                table: "ListingExchanges",
                column: "Code",
                value: "CNSX");

            migrationBuilder.InsertData(
                table: "ListingExchanges",
                column: "Code",
                value: "MX");

            migrationBuilder.InsertData(
                table: "ListingExchanges",
                column: "Code",
                value: "NASDAQ");

            migrationBuilder.InsertData(
                table: "ListingExchanges",
                column: "Code",
                value: "NYSE");

            migrationBuilder.InsertData(
                table: "ListingExchanges",
                column: "Code",
                value: "NYSEAM");

            migrationBuilder.InsertData(
                table: "ListingExchanges",
                column: "Code",
                value: "OPRA");

            migrationBuilder.InsertData(
                table: "ListingExchanges",
                column: "Code",
                value: "OTCBB");

            migrationBuilder.InsertData(
                table: "ListingExchanges",
                column: "Code",
                value: "PinkSheets");

            migrationBuilder.InsertData(
                table: "ListingExchanges",
                column: "Code",
                value: "TSX");

            migrationBuilder.InsertData(
                table: "ListingExchanges",
                column: "Code",
                value: "TSXV");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_ConnectionId",
                table: "Accounts",
                column: "ConnectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_Number_ConnectionId",
                table: "Accounts",
                columns: new[] { "Number", "ConnectionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AllocationProportions_AllocationId_AssetClassId",
                table: "AllocationProportions",
                columns: new[] { "AllocationId", "AssetClassId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AllocationProportions_AssetClassId",
                table: "AllocationProportions",
                column: "AssetClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Allocations_PortfolioId_SymbolId",
                table: "Allocations",
                columns: new[] { "PortfolioId", "SymbolId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Allocations_SymbolId",
                table: "Allocations",
                column: "SymbolId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetClasses_Name_PortfolioId",
                table: "AssetClasses",
                columns: new[] { "Name", "PortfolioId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AssetClasses_PortfolioId",
                table: "AssetClasses",
                column: "PortfolioId");

            migrationBuilder.CreateIndex(
                name: "IX_Balances_AccountId",
                table: "Balances",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Balances_CurrencyCode",
                table: "Balances",
                column: "CurrencyCode");

            migrationBuilder.CreateIndex(
                name: "IX_Balances_Type_CurrencyCode_AccountId",
                table: "Balances",
                columns: new[] { "Type", "CurrencyCode", "AccountId" },
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

            migrationBuilder.CreateIndex(
                name: "IX_Connections_BrokerageId_BrokerageUserId_UserId",
                table: "Connections",
                columns: new[] { "BrokerageId", "BrokerageUserId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Connections_UserId",
                table: "Connections",
                column: "UserId");

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

            migrationBuilder.CreateIndex(
                name: "IX_Portfolios_UserId",
                table: "Portfolios",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Positions_AccountId",
                table: "Positions",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Positions_SymbolId_AccountId",
                table: "Positions",
                columns: new[] { "SymbolId", "AccountId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Symbols_CurrencyCode",
                table: "Symbols",
                column: "CurrencyCode");

            migrationBuilder.CreateIndex(
                name: "IX_Symbols_ListingExchangeCode",
                table: "Symbols",
                column: "ListingExchangeCode");

            migrationBuilder.CreateIndex(
                name: "IX_Symbols_Name",
                table: "Symbols",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AllocationProportions");

            migrationBuilder.DropTable(
                name: "Balances");

            migrationBuilder.DropTable(
                name: "BrokerageSymbols");

            migrationBuilder.DropTable(
                name: "ExcludeAccounts");

            migrationBuilder.DropTable(
                name: "ExcludeSymbols");

            migrationBuilder.DropTable(
                name: "Positions");

            migrationBuilder.DropTable(
                name: "Reports");

            migrationBuilder.DropTable(
                name: "Allocations");

            migrationBuilder.DropTable(
                name: "AssetClasses");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Symbols");

            migrationBuilder.DropTable(
                name: "Portfolios");

            migrationBuilder.DropTable(
                name: "Connections");

            migrationBuilder.DropTable(
                name: "Currencies");

            migrationBuilder.DropTable(
                name: "ListingExchanges");

            migrationBuilder.DropTable(
                name: "Brokerages");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
