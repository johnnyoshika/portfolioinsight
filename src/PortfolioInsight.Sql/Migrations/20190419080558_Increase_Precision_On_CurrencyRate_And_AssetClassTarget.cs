using Microsoft.EntityFrameworkCore.Migrations;

namespace PortfolioInsight.Migrations
{
    public partial class Increase_Precision_On_CurrencyRate_And_AssetClassTarget : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Rate",
                table: "Currencies",
                type: "decimal(14, 9)",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "Target",
                table: "AssetClasses",
                type: "decimal(4, 3)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Rate",
                table: "Currencies",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(14, 9)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Target",
                table: "AssetClasses",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(4, 3)",
                oldNullable: true);
        }
    }
}
