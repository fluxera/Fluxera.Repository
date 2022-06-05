using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fluxera.Repository.EntityFrameworkCore.IntegrationTests.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    ID = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    LegalType = table.Column<string>(type: "TEXT", nullable: false),
                    Guid = table.Column<Guid>(type: "TEXT", nullable: false),
                    NullableGuid = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    ID = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    SalaryInt = table.Column<int>(type: "INTEGER", nullable: false),
                    SalaryLong = table.Column<long>(type: "INTEGER", nullable: false),
                    SalaryDecimal = table.Column<double>(type: "REAL", nullable: false),
                    SalaryFloat = table.Column<float>(type: "REAL", nullable: false),
                    SalaryDouble = table.Column<double>(type: "REAL", nullable: false),
                    SalaryNullableInt = table.Column<int>(type: "INTEGER", nullable: true),
                    SalaryNullableLong = table.Column<long>(type: "INTEGER", nullable: true),
                    SalaryNullableDecimal = table.Column<double>(type: "REAL", nullable: true),
                    SalaryNullableFloat = table.Column<float>(type: "REAL", nullable: true),
                    SalaryNullableDouble = table.Column<double>(type: "REAL", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "People",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Age = table.Column<int>(type: "INTEGER", nullable: false),
                    Address_Street = table.Column<string>(type: "TEXT", nullable: true),
                    Address_Number = table.Column<string>(type: "TEXT", nullable: true),
                    Address_City = table.Column<string>(type: "TEXT", nullable: true),
                    Address_PostCode = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_People", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "People");
        }
    }
}
