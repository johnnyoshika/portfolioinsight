using Microsoft.EntityFrameworkCore.Migrations;

namespace PortfolioInsight.Migrations
{
    public partial class Balances_CurrencyCode_ForeignKey_Constraint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Balances_CurrencyCode",
                table: "Balances",
                column: "CurrencyCode");

            migrationBuilder.CreateIndex(
                name: "IX_Balances_Type_CurrencyCode_AccountId",
                table: "Balances",
                columns: new[] { "Type", "CurrencyCode", "AccountId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Balances_Currencies_CurrencyCode",
                table: "Balances",
                column: "CurrencyCode",
                principalTable: "Currencies",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Balances_Currencies_CurrencyCode",
                table: "Balances");

            migrationBuilder.DropIndex(
                name: "IX_Balances_CurrencyCode",
                table: "Balances");

            migrationBuilder.DropIndex(
                name: "IX_Balances_Type_CurrencyCode_AccountId",
                table: "Balances");
        }
    }
}
