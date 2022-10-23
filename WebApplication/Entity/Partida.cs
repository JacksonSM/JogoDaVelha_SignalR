using Newtonsoft.Json;
using WebApplication_Jogo.Entity.Execptions;
using WebApplication_Jogo.Services;

namespace WebApplication_Jogo.Entity;

public class Partida
{
    public int Id { get; set; }

    public Jogador JogadorLocal { get; private set; }

    public Jogador JogadorFora { get; private set; }

    public string CodigoPartida { get; private set; }

    public string JogadorDaVezConnectionId { get; private set; }

    public Partida() { }

    public Partida(Jogador jogadorLocal)
    {
        JogadorLocal = jogadorLocal;
        JogadorDaVezConnectionId = JogadorLocal.ConnectionId;
        CodigoPartida = GeradorDeCodigo.Gerar();
    }

    public void ConectarJogadorFora(Jogador jogadorFora)
    {
        if (JogadorFora != null)
            throw new JogoDaVelhaExceptions("A partida não existe ou está completa.");

        JogadorFora = jogadorFora;
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
