using Game.Services;
using Newtonsoft.Json;
using WebApplication_Jogo.Entity.Execptions;

namespace Game.Entity;

public class Partida
{
    public int Id { get; set; }

    public Jogador JogadorLocal { get; private set; }

    public Jogador JogadorFora { get; private set; }

    public string CodigoPartida { get; private set; }

    public Tabuleiro Tabuleiro { get; private set; } = new();

    public string JogadorDaVezConnectionId { get; private set; }

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

    public void MarcarPosicao(int posicao, string connectionId)
    {
        if (!connectionId.Equals(JogadorDaVezConnectionId))
            throw new RegrasExceptions("Não é a vez do jogador.");

        string marca = connectionId.Equals(JogadorLocal.ConnectionId)
            ? JogadorLocal.Marca : JogadorFora.Marca;

        Tabuleiro.MarcarPosicao(marca, posicao);

        TrocarAVez();
    }

    public string JogadorDaVez()
    {
        string jogadorId = string.Empty;

        if (JogadorFora != null)
        {
            jogadorId = JogadorDaVezConnectionId;

            TrocarAVez();
        }

        return jogadorId;
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
}
