using Game.Hubs;
using Game.Models.Execptions;
using Game.Models.Tools;
using Newtonsoft.Json;
using System.Numerics;

namespace Game.Models;

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
            throw new GameException("A partida não existe ou está completa.");
        jogadorFora.Marca = "O";
        JogadorFora = jogadorFora;
    }

    /// <summary>
    /// Atribui a marca do jogador da vez em uma posicão no tabuleiro
    /// </summary>
    /// <param name="posicao">A posicão no tabuleiro que deve se atribuida a marca</param>
    /// <param name="connectionId">ConnectionId do usuario que desaja marcar o tabuleiro</param>
    /// <param name="hub">O hub para caso empate ou vitoria, invocar o fim da partida</param>
    public async void MarcarPosicao(Vector2 posicao, string connectionId, PartidaHub hub)
    {
        if (!connectionId.Equals(JogadorDaVezConnectionId))
            throw new GameException("Não é a vez do jogador.");

        Hub = hub;

        string marca = ObterMarcaJogadorDaVez();

        Tabuleiro.MarcarPosicao(marca, posicao);

        var isEmpate = Tabuleiro.VerificarEmpate();
        if (isEmpate)
            await Empate();

        var isVitoria = Tabuleiro.VerificarVitoria();
        if (isVitoria)
            await Vitoria();

        TrocarAVez();
    }

    /// <summary>
    /// Finaliza a partida declarando um vencedor.
    /// </summary>
    private async Task Vitoria()
    {
        var nomeJogadorVez = JogadorDaVezConnectionId.Equals(JogadorLocal.ConnectionId)
            ? JogadorLocal.Nome : JogadorFora.Nome;

        await Hub.FimJogoVitoria(this, nomeJogadorVez);
    }

    /// <summary>
    /// Finaliza a partida declarando o empate.
    /// </summary>
    private async Task Empate()
    {
        await Hub.FimJogoEmpate(this);
    }

    /// <summary>
    /// Este metodo serializar o seu objeto.
    /// </summary>
    /// <returns>Retorna o proprio objeto serializado</returns>
    public string Serializar()
    {
        return JsonConvert.SerializeObject(this);
    }

    /// <summary>
    /// Setar os valores da partida e tabuleiro aos valores padrão.
    /// </summary>
    public void Resetar()
    {
        JogadorDaVezConnectionId = JogadorLocal.ConnectionId;
        Tabuleiro.Resetar();
    }

    private string ObterMarcaJogadorDaVez() =>
     JogadorDaVezConnectionId.Equals(JogadorLocal.ConnectionId)
        ? JogadorLocal.Marca : JogadorFora.Marca;

    /// <summary>
    /// Alterna o valor do campo JogadorDaVezConnectionId entre a connectionId do JogadorLocal e JogadorFora.
    /// </summary>
    private void TrocarAVez()
    {
        JogadorDaVezConnectionId = JogadorDaVezConnectionId.Equals(JogadorLocal.ConnectionId) ?
                JogadorFora.ConnectionId : JogadorLocal.ConnectionId;
    }
}
