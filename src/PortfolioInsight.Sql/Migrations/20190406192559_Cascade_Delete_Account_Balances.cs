using Microsoft.EntityFrameworkCore.Migrations;

namespace PortfolioInsight.Migrations
{
    public partial class Cascade_Delete_Account_Balances : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Balances_Accounts_AccountId",
                table: "Balances");

            migrationBuilder.AddForeignKey(
                name: "FK_Balances_Accounts_AccountId",
                table: "Balances",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Balances_Accounts_AccountId",
                table: "Balances");

            migrationBuilder.AddForeignKey(
                name: "FK_Balances_Accounts_AccountId",
                table: "Balances",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
