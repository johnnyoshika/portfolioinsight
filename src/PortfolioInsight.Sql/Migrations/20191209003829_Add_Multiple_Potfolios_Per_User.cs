using Microsoft.EntityFrameworkCore.Migrations;

namespace PortfolioInsight.Migrations
{
    public partial class Add_Multiple_Potfolios_Per_User : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AllocationProportions_AssetClasses_AssetClassId",
                table: "AllocationProportions");

            migrationBuilder.DropForeignKey(
                name: "FK_Allocations_Users_UserId",
                table: "Allocations");

            migrationBuilder.DropForeignKey(
                name: "FK_AssetClasses_Users_UserId",
                table: "AssetClasses");

            migrationBuilder.DropIndex(
                name: "IX_AssetClasses_UserId",
                table: "AssetClasses");

            migrationBuilder.DropIndex(
                name: "IX_AssetClasses_Name_UserId",
                table: "AssetClasses");

            migrationBuilder.DropIndex(
                name: "IX_Allocations_UserId_SymbolId",
                table: "Allocations");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "AssetClasses");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Allocations");

            migrationBuilder.AddColumn<int>(
                name: "PortfolioId",
                table: "AssetClasses",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PortfolioId",
                table: "Allocations",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Portfolios",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    UserId = table.Column<int>(nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_AssetClasses_PortfolioId",
                table: "AssetClasses",
                column: "PortfolioId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetClasses_Name_PortfolioId",
                table: "AssetClasses",
                columns: new[] { "Name", "PortfolioId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Allocations_PortfolioId_SymbolId",
                table: "Allocations",
                columns: new[] { "PortfolioId", "SymbolId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Portfolios_UserId",
                table: "Portfolios",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AllocationProportions_AssetClasses_AssetClassId",
                table: "AllocationProportions",
                column: "AssetClassId",
                principalTable: "AssetClasses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Allocations_Portfolios_PortfolioId",
                table: "Allocations",
                column: "PortfolioId",
                principalTable: "Portfolios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AssetClasses_Portfolios_PortfolioId",
                table: "AssetClasses",
                column: "PortfolioId",
                principalTable: "Portfolios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AllocationProportions_AssetClasses_AssetClassId",
                table: "AllocationProportions");

            migrationBuilder.DropForeignKey(
                name: "FK_Allocations_Portfolios_PortfolioId",
                table: "Allocations");

            migrationBuilder.DropForeignKey(
                name: "FK_AssetClasses_Portfolios_PortfolioId",
                table: "AssetClasses");

            migrationBuilder.DropTable(
                name: "Portfolios");

            migrationBuilder.DropIndex(
                name: "IX_AssetClasses_PortfolioId",
                table: "AssetClasses");

            migrationBuilder.DropIndex(
                name: "IX_AssetClasses_Name_PortfolioId",
                table: "AssetClasses");

            migrationBuilder.DropIndex(
                name: "IX_Allocations_PortfolioId_SymbolId",
                table: "Allocations");

            migrationBuilder.DropColumn(
                name: "PortfolioId",
                table: "AssetClasses");

            migrationBuilder.DropColumn(
                name: "PortfolioId",
                table: "Allocations");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "AssetClasses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Allocations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AssetClasses_UserId",
                table: "AssetClasses",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetClasses_Name_UserId",
                table: "AssetClasses",
                columns: new[] { "Name", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Allocations_UserId_SymbolId",
                table: "Allocations",
                columns: new[] { "UserId", "SymbolId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AllocationProportions_AssetClasses_AssetClassId",
                table: "AllocationProportions",
                column: "AssetClassId",
                principalTable: "AssetClasses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Allocations_Users_UserId",
                table: "Allocations",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AssetClasses_Users_UserId",
                table: "AssetClasses",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
