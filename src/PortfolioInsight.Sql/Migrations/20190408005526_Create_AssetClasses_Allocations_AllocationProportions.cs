using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PortfolioInsight.Migrations
{
    public partial class Create_AssetClasses_Allocations_AllocationProportions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Allocations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(nullable: false),
                    SymbolId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Allocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Allocations_Symbols_SymbolId",
                        column: x => x.SymbolId,
                        principalTable: "Symbols",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Allocations_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AssetClasses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Target = table.Column<decimal>(nullable: true),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssetClasses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssetClasses_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AllocationProportions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AllocationId = table.Column<int>(nullable: false),
                    AssetClassId = table.Column<int>(nullable: false),
                    Rate = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AllocationProportions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AllocationProportions_Allocations_AllocationId",
                        column: x => x.AllocationId,
                        principalTable: "Allocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AllocationProportions_AssetClasses_AssetClassId",
                        column: x => x.AssetClassId,
                        principalTable: "AssetClasses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AllocationProportions_AssetClassId",
                table: "AllocationProportions",
                column: "AssetClassId");

            migrationBuilder.CreateIndex(
                name: "IX_AllocationProportions_AllocationId_AssetClassId",
                table: "AllocationProportions",
                columns: new[] { "AllocationId", "AssetClassId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Allocations_SymbolId",
                table: "Allocations",
                column: "SymbolId");

            migrationBuilder.CreateIndex(
                name: "IX_Allocations_UserId_SymbolId",
                table: "Allocations",
                columns: new[] { "UserId", "SymbolId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AssetClasses_UserId",
                table: "AssetClasses",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetClasses_Name_UserId",
                table: "AssetClasses",
                columns: new[] { "Name", "UserId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AllocationProportions");

            migrationBuilder.DropTable(
                name: "Allocations");

            migrationBuilder.DropTable(
                name: "AssetClasses");
        }
    }
}
