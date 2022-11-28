using Game.Entity.Execptions;
using Game.Services;
using Newtonsoft.Json;
using System.Numerics;

namespace Game.Entity;

public class Partida
{
    public int Id { get; set; }

    public Jogador JogadorLocal { get; private set; }

    public Jogador JogadorFora { get; private set; }

    public string CodigoPartida { get; private set; }

    public Tabuleiro Tabuleiro { get; private set; } = new();

    public string JogadorDaVezConnectionId { get; private set; }


    public delegate Task FimDeJogo(Partida partida, string vencedor);

    public Partida() { }

    public Partida(Jogador jogadorLocal)
    {
        JogadorLocal = jogadorLocal;
        JogadorLocal.Marca = "X";
        JogadorDaVezConnectionId = JogadorLocal.ConnectionId;
        CodigoPartida = GeradorDeCodigo.Gerar();
    }

    public void ConectarJogadorFora(Jogador jogadorFora)
    {
        if (JogadorFora != null)
            throw new RegrasExceptions("A partida não existe ou está completa.");
        jogadorFora.Marca = "O";
        JogadorFora = jogadorFora;
    }

    public void MarcarPosicao(Vector2 posicao, string connectionId, FimDeJogo fimDeJogo)
    {
        if (!connectionId.Equals(JogadorDaVezConnectionId))
            throw new RegrasExceptions("Não é a vez do jogador.");

        string marca = connectionId.Equals(JogadorLocal.ConnectionId)
            ? JogadorLocal.Marca : JogadorFora.Marca;

        Tabuleiro.MarcarPosicao(marca, posicao);

        var empate = Tabuleiro.VerificarEmapate();

        if (empate)
        {


            fimDeJogo(this, "Empate");
        }

        if (Tabuleiro.PosicoesIguais != null)
        {
            var nomeJogadorVez = connectionId.Equals(JogadorLocal.ConnectionId)
            ? JogadorLocal.Nome : JogadorFora.Nome;

            fimDeJogo(this, nomeJogadorVez);
        }
            

        TrocarAVez();
    }

    public string Serializar()
    {
        return JsonConvert.SerializeObject(this);
    }

    private void TrocarAVez()
    {
        JogadorDaVezConnectionId = JogadorDaVezConnectionId.Equals(JogadorLocal.ConnectionId) ?
                JogadorFora.ConnectionId : JogadorLocal.ConnectionId;
    }

    public void Resetar()
    {
        JogadorDaVezConnectionId = JogadorLocal.ConnectionId;
        Tabuleiro.Resetar();
    }
}
