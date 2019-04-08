using Microsoft.EntityFrameworkCore.Migrations;

namespace PortfolioInsight.Migrations
{
    public partial class Add_Currency_Nullable_Rate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Rate",
                table: "Currencies",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rate",
                table: "Currencies");
        }
    }
}
