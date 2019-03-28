using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PortfolioInsight.Migrations
{
    public partial class Initial_Create : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Brokerages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brokerages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    Code = table.Column<string>(maxLength: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    LastActivityAt = table.Column<DateTime>(nullable: true),
                    ActivityCount = table.Column<int>(nullable: false),
                    LastLoginAt = table.Column<DateTime>(nullable: true),
                    LoginCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Authorizations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BrokerageId = table.Column<int>(nullable: false),
                    BrokerageUserId = table.Column<string>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    RefreshToken = table.Column<string>(nullable: false),
                    IsValid = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authorizations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Authorizations_Brokerages_BrokerageId",
                        column: x => x.BrokerageId,
                        principalTable: "Brokerages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Authorizations_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Number = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    AuthorizationId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Accounts_Authorizations_AuthorizationId",
                        column: x => x.AuthorizationId,
                        principalTable: "Authorizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Positions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Ticker = table.Column<string>(maxLength: 10, nullable: false),
                    Value = table.Column<decimal>(type: "money", nullable: false),
                    CurrencyCode = table.Column<string>(maxLength: 3, nullable: false),
                    AccountId = table.Column<int>(nullable: false)
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
                        name: "FK_Positions_Currencies_CurrencyCode",
                        column: x => x.CurrencyCode,
                        principalTable: "Currencies",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Brokerages",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Questrade" });

            migrationBuilder.InsertData(
                table: "Currencies",
                column: "Code",
                value: "CAD");

            migrationBuilder.InsertData(
                table: "Currencies",
                column: "Code",
                value: "USA");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_AuthorizationId",
                table: "Accounts",
                column: "AuthorizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_Number_AuthorizationId",
                table: "Accounts",
                columns: new[] { "Number", "AuthorizationId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Authorizations_UserId",
                table: "Authorizations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Authorizations_BrokerageId_BrokerageUserId_UserId",
                table: "Authorizations",
                columns: new[] { "BrokerageId", "BrokerageUserId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Positions_AccountId",
                table: "Positions",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Positions_CurrencyCode",
                table: "Positions",
                column: "CurrencyCode");

            migrationBuilder.CreateIndex(
                name: "IX_Positions_Ticker_AccountId",
                table: "Positions",
                columns: new[] { "Ticker", "AccountId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Positions");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Currencies");

            migrationBuilder.DropTable(
                name: "Authorizations");

            migrationBuilder.DropTable(
                name: "Brokerages");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
