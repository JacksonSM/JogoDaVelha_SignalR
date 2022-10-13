using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication_Jogo.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Partidas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    JogadorLocal_Nome = table.Column<string>(type: "TEXT", nullable: true),
                    JogadorLocal_ConnectionIdd = table.Column<string>(type: "TEXT", nullable: true),
                    JogadorFora_Nome = table.Column<string>(type: "TEXT", nullable: true),
                    JogadorFora_ConnectionIdd = table.Column<string>(type: "TEXT", nullable: true),
                    CodigoPartida = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Partidas", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Partidas");
        }
    }
}
