using Microsoft.EntityFrameworkCore.Migrations;

namespace PortfolioInsight.Migrations
{
    public partial class Drop_Symbols_ListingExchange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ListingExchange",
                table: "Symbols");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ListingExchange",
                table: "Symbols",
                maxLength: 10,
                nullable: false,
                defaultValue: "");
        }
    }
}
