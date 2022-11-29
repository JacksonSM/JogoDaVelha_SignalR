using Game.Entity.Execptions;
using Game.Hubs;
using Game.Services;
using Newtonsoft.Json;
using System.Numerics;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Game.Entity;

public class Partida
{
    public int Id { get; set; }

    public Jogador JogadorLocal { get; private set; }

    public Jogador JogadorFora { get; private set; }

    public string CodigoPartida { get; private set; }

    public Tabuleiro Tabuleiro { get; private set; } = new();

    public string JogadorDaVezConnectionId { get; private set; }

    [JsonIgnore]
    public PartidaHub Hub;

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

    public async void MarcarPosicao(Vector2 posicao, string connectionId, PartidaHub hub)
    {
        if (!connectionId.Equals(JogadorDaVezConnectionId))
            throw new RegrasExceptions("Não é a vez do jogador.");

        Hub = hub;

        string marca = ObterMarcaJogadorDaVez();
            
        Tabuleiro.MarcarPosicao(marca, posicao);

        var isEmpate = Tabuleiro.VerificarEmpate();
        if (isEmpate)
            await Empate();

        var isVitoria = Tabuleiro.VerificarVitoria();
        if(isVitoria)
            await Vitoria();

        TrocarAVez();
    }

    /// <summary>
    /// Finaliza a partida declarando um vencedor.
    /// </summary>
    public async Task Vitoria()
    {
        var nomeJogadorVez = JogadorDaVezConnectionId.Equals(JogadorLocal.ConnectionId)
            ? JogadorLocal.Nome : JogadorFora.Nome;

        await Hub.FimJogoVitoria(this, nomeJogadorVez);
    }

    /// <summary>
    /// Finaliza a partida declarando o empate.
    /// </summary>
    public async Task Empate()
    {
        await Hub.FimJogoEmpate(this);
    }

    public string Serializar()
    {
        return JsonConvert.SerializeObject(this);
    }

    public void Resetar()
    {
        JogadorDaVezConnectionId = JogadorLocal.ConnectionId;
        Tabuleiro.Resetar();
    }

    private string ObterMarcaJogadorDaVez() =>
     JogadorDaVezConnectionId.Equals(JogadorLocal.ConnectionId)
        ? JogadorLocal.Marca : JogadorFora.Marca;

    private void TrocarAVez()
    {
        JogadorDaVezConnectionId = JogadorDaVezConnectionId.Equals(JogadorLocal.ConnectionId) ?
                JogadorFora.ConnectionId : JogadorLocal.ConnectionId;
    }
}
