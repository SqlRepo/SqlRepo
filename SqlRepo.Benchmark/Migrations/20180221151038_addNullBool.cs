using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SqlRepo.Benchmark.Migrations
{
    public partial class addNullBool : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "NullableBoolean",
                table: "BenchmarkEntity",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NullableBoolean",
                table: "BenchmarkEntity");
        }
    }
}
