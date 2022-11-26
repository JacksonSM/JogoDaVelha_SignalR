using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication_Jogo.Migrations
{
    public partial class AdicionadoColunaJogadorDaVezConnectionIdnaTabelaPartida : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "JogadorDaVezConnectionId",
                table: "Partidas",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JogadorFora_Marca",
                table: "Partidas",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JogadorLocal_Marca",
                table: "Partidas",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JogadorDaVezConnectionId",
                table: "Partidas");

            migrationBuilder.DropColumn(
                name: "JogadorFora_Marca",
                table: "Partidas");

            migrationBuilder.DropColumn(
                name: "JogadorLocal_Marca",
                table: "Partidas");
        }
    }
}
