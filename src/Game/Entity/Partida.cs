using Game.Entity.Execptions;
using Game.Hubs;
using Game.Services;
using Newtonsoft.Json;
using System.Numerics;

namespace Game.Entity;

public class Partida
{
    [JsonProperty]
    public int Id { get; set; }
    [JsonProperty]
    public Jogador JogadorLocal { get; private set; }
    [JsonProperty]
    public Jogador JogadorFora { get; private set; }
    [JsonProperty]
    public string CodigoPartida { get; private set; }
    [JsonProperty]
    public Tabuleiro Tabuleiro { get; private set; } = new();
    [JsonProperty]
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
            throw new GameExceptions("A partida não existe ou está completa.");
        jogadorFora.Marca = "O";
        JogadorFora = jogadorFora;
    }

    public async void MarcarPosicao(Vector2 posicao, string connectionId, PartidaHub hub)
    {
        if (!connectionId.Equals(JogadorDaVezConnectionId))
            throw new GameExceptions("Não é a vez do jogador.");

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
