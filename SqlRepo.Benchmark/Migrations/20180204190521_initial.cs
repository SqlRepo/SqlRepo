using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SqlRepo.Benchmark.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BenchmarkEntity",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DecimalValue = table.Column<decimal>(nullable: false),
                    IntegerValue = table.Column<int>(nullable: false),
                    TextValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BenchmarkEntity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BenchmarkResult",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Component = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    Notes = table.Column<string>(nullable: true),
                    TestName = table.Column<string>(nullable: true),
                    TimeTaken = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BenchmarkResult", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BenchmarkEntity");

            migrationBuilder.DropTable(
                name: "BenchmarkResult");
        }
    }
}
