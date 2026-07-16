using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FacturaService.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Facturi",
                columns: table => new
                {
                    NrFactura = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdClient = table.Column<int>(type: "int", nullable: false),
                    DataFactura = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Observatii = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Facturi", x => x.NrFactura);
                });

            migrationBuilder.CreateTable(
                name: "ProdusFactura",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NrFactura = table.Column<int>(type: "int", nullable: false),
                    IdProdus = table.Column<int>(type: "int", nullable: false),
                    Denumire = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PretUnitar = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Cantitate = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProdusFactura", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProdusFactura_Facturi_NrFactura",
                        column: x => x.NrFactura,
                        principalTable: "Facturi",
                        principalColumn: "NrFactura",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProdusFactura_NrFactura",
                table: "ProdusFactura",
                column: "NrFactura");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProdusFactura");

            migrationBuilder.DropTable(
                name: "Facturi");
        }
    }
}
