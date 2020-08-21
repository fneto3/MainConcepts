using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Infrastructure.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CalculatorType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(12)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalculatorType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Calculator",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CalculatorTypeId = table.Column<int>(type: "int", nullable: false),
                    A = table.Column<decimal>(type: "decimal(12,4)", nullable: false),
                    B = table.Column<decimal>(type: "decimal(12,4)", nullable: false),
                    Result = table.Column<decimal>(type: "decimal(12,4)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Baskets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Calculator_CalculatorType",
                        column: _ => _.CalculatorTypeId,
                        principalTable: "CalculationType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });
        }
    }
}
