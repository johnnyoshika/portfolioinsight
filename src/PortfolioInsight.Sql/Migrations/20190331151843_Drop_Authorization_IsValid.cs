using Microsoft.EntityFrameworkCore.Migrations;

namespace PortfolioInsight.Migrations
{
    public partial class Drop_Authorization_IsValid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsValid",
                table: "Authorizations");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsValid",
                table: "Authorizations",
                nullable: false,
                defaultValue: false);
        }
    }
}
