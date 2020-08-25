using Microsoft.EntityFrameworkCore.Migrations;

namespace Calculator.API.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CalculatorTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalculatorTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Calculators",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    A = table.Column<decimal>(nullable: false),
                    B = table.Column<decimal>(nullable: false),
                    Result = table.Column<decimal>(nullable: false),
                    CalculatorTypeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Calculators", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Calculators_CalculatorTypes_CalculatorTypeId",
                        column: x => x.CalculatorTypeId,
                        principalTable: "CalculatorTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Calculators_CalculatorTypeId",
                table: "Calculators",
                column: "CalculatorTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Calculators");

            migrationBuilder.DropTable(
                name: "CalculatorTypes");
        }
    }
}
