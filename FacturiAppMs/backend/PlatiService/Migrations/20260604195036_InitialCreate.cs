using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlatiService.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
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
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Plati");
        }
    }
}
