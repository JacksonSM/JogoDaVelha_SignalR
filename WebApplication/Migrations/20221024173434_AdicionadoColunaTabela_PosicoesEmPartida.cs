using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication_Jogo.Migrations
{
    public partial class AdicionadoColunaTabela_PosicoesEmPartida : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Tabuleiro_Posicoes",
                table: "Partidas",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tabuleiro_Posicoes",
                table: "Partidas");
        }
    }
}
