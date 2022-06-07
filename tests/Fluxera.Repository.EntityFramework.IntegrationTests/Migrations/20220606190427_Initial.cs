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
                    NullableGuid = table.Column<Guid>(type: "TEXT", nullable: true),
                    ReferenceID = table.Column<string>(type: "TEXT", nullable: true)
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
                    SalaryNullableDouble = table.Column<double>(type: "REAL", nullable: true),
                    ReferenceID = table.Column<string>(type: "TEXT", nullable: true)
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
                    Address_PostCode = table.Column<string>(type: "TEXT", nullable: true),
                    ReferenceID = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_People", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "References",
                columns: table => new
                {
                    ID = table.Column<string>(type: "TEXT", nullable: false),
                    CompanyID = table.Column<string>(type: "TEXT", nullable: true),
                    PersonID = table.Column<Guid>(type: "TEXT", nullable: true),
                    EmployeeID = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_References", x => x.ID);
                    table.ForeignKey(
                        name: "FK_References_Companies_CompanyID",
                        column: x => x.CompanyID,
                        principalTable: "Companies",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_References_Employees_EmployeeID",
                        column: x => x.EmployeeID,
                        principalTable: "Employees",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_References_People_PersonID",
                        column: x => x.PersonID,
                        principalTable: "People",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Companies_ReferenceID",
                table: "Companies",
                column: "ReferenceID");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_ReferenceID",
                table: "Employees",
                column: "ReferenceID");

            migrationBuilder.CreateIndex(
                name: "IX_People_ReferenceID",
                table: "People",
                column: "ReferenceID");

            migrationBuilder.CreateIndex(
                name: "IX_References_CompanyID",
                table: "References",
                column: "CompanyID");

            migrationBuilder.CreateIndex(
                name: "IX_References_EmployeeID",
                table: "References",
                column: "EmployeeID");

            migrationBuilder.CreateIndex(
                name: "IX_References_PersonID",
                table: "References",
                column: "PersonID");

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_References_ReferenceID",
                table: "Companies",
                column: "ReferenceID",
                principalTable: "References",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_References_ReferenceID",
                table: "Employees",
                column: "ReferenceID",
                principalTable: "References",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_People_References_ReferenceID",
                table: "People",
                column: "ReferenceID",
                principalTable: "References",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_References_ReferenceID",
                table: "Companies");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_References_ReferenceID",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_People_References_ReferenceID",
                table: "People");

            migrationBuilder.DropTable(
                name: "References");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "People");
        }
    }
}
