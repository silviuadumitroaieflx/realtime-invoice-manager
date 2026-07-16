using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FacturiApp.Migrations
{
    /// <inheritdoc />
    public partial class AddAuthSiPlati : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Plati",
                columns: table => new
                {
                    IdPlata = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NrFactura = table.Column<int>(type: "int", nullable: false),
                    Suma = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DataPlata = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MetodaPlata = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plati", x => x.IdPlata);
                    table.ForeignKey(
                        name: "FK_Plati_Facturi_NrFactura",
                        column: x => x.NrFactura,
                        principalTable: "Facturi",
                        principalColumn: "NrFactura",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    IdUser = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nume = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.IdUser);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Plati_NrFactura",
                table: "Plati",
                column: "NrFactura");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Plati");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
