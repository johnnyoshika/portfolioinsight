using Microsoft.EntityFrameworkCore.Migrations;

namespace PortfolioInsight.Migrations
{
    public partial class Make_Currency_Rate_Non_Nullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Rate",
                table: "Currencies",
                nullable: false,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "CAD",
                column: "Rate",
                value: 0m);

            migrationBuilder.UpdateData(
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "USD",
                column: "Rate",
                value: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Rate",
                table: "Currencies",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.UpdateData(
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "CAD",
                column: "Rate",
                value: null);

            migrationBuilder.UpdateData(
                table: "Currencies",
                keyColumn: "Code",
                keyValue: "USD",
                column: "Rate",
                value: null);
        }
    }
}
