using Microsoft.EntityFrameworkCore.Migrations;

namespace PortfolioInsight.Migrations
{
    public partial class Rename_Authorizations_To_Connections : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_Authorizations_AuthorizationId",
                table: "Accounts");

            migrationBuilder.DropTable(
                name: "Authorizations");

            migrationBuilder.RenameColumn(
                name: "AuthorizationId",
                table: "Accounts",
                newName: "ConnectionId");

            migrationBuilder.RenameIndex(
                name: "IX_Accounts_Number_AuthorizationId",
                table: "Accounts",
                newName: "IX_Accounts_Number_ConnectionId");

            migrationBuilder.RenameIndex(
                name: "IX_Accounts_AuthorizationId",
                table: "Accounts",
                newName: "IX_Accounts_ConnectionId");

            migrationBuilder.CreateTable(
                name: "Connections",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BrokerageId = table.Column<int>(nullable: false),
                    BrokerageUserId = table.Column<string>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    RefreshToken = table.Column<string>(nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_Connections_UserId",
                table: "Connections",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Connections_BrokerageId_BrokerageUserId_UserId",
                table: "Connections",
                columns: new[] { "BrokerageId", "BrokerageUserId", "UserId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_Connections_ConnectionId",
                table: "Accounts",
                column: "ConnectionId",
                principalTable: "Connections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_Connections_ConnectionId",
                table: "Accounts");

            migrationBuilder.DropTable(
                name: "Connections");

            migrationBuilder.RenameColumn(
                name: "ConnectionId",
                table: "Accounts",
                newName: "AuthorizationId");

            migrationBuilder.RenameIndex(
                name: "IX_Accounts_Number_ConnectionId",
                table: "Accounts",
                newName: "IX_Accounts_Number_AuthorizationId");

            migrationBuilder.RenameIndex(
                name: "IX_Accounts_ConnectionId",
                table: "Accounts",
                newName: "IX_Accounts_AuthorizationId");

            migrationBuilder.CreateTable(
                name: "Authorizations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BrokerageId = table.Column<int>(nullable: false),
                    BrokerageUserId = table.Column<string>(nullable: false),
                    RefreshToken = table.Column<string>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_Authorizations_UserId",
                table: "Authorizations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Authorizations_BrokerageId_BrokerageUserId_UserId",
                table: "Authorizations",
                columns: new[] { "BrokerageId", "BrokerageUserId", "UserId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_Authorizations_AuthorizationId",
                table: "Accounts",
                column: "AuthorizationId",
                principalTable: "Authorizations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
