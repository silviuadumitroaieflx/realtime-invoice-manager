using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProdusService.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Produse",
                columns: table => new
                {
                    IdProdus = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumeProdus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PretUnitar = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UnitateMasura = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produse", x => x.IdProdus);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Produse");
        }
    }
}
