using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fluxera.Repository.EntityFrameworkCore.IntegrationTests.Migrations
{
    public partial class Invoices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CustomerName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceItem",
                columns: table => new
                {
                    ID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    InvoiceID = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceItem", x => x.ID);
                    table.ForeignKey(
                        name: "FK_InvoiceItem_Invoices_InvoiceID",
                        column: x => x.InvoiceID,
                        principalTable: "Invoices",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceItem_InvoiceID",
                table: "InvoiceItem",
                column: "InvoiceID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvoiceItem");

            migrationBuilder.DropTable(
                name: "Invoices");
        }
    }
}
