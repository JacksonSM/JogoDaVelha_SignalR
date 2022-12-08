using Game.Context;
using Game.Models;
using Game.Models.Execptions;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System.Numerics;

namespace Game.Hubs;

public class PartidaHub : Hub
{

    private readonly IPartidaRepository _partidaRepository;

    public PartidaHub(IPartidaRepository partidaRepository)
    {
        _partidaRepository = partidaRepository;
    }

    /// <summary>
    /// Quando o usuário se desconectar da partida, o seu oponente será avisado, caso 
    /// o usuário que desconectou for o dono da partida, a partida vai ser removida da memoria.
    /// </summary>
    public override async Task OnDisconnectedAsync(Exception exception)
    {
        var partida = await _partidaRepository.ObterPartidaPorJogadorAsync(Context.ConnectionId);

        if (partida != null)
        {
            var adversarioConnectionId = partida.JogadorLocal.ConnectionId == Context.ConnectionId ? 
                partida.JogadorFora?.ConnectionId : partida.JogadorLocal?.ConnectionId;

            await Clients.Clients(adversarioConnectionId).SendAsync("AdversarioDesconectado");

            await _partidaRepository.RemoverAsync(partida);
        }

        await base.OnDisconnectedAsync(exception);
    }

    /// <summary>
    /// Criado e armazenado a partida na memória.
    /// </summary>
    /// <param name="nome">Nome do jogador</param>
    public async Task CriarPartida(string nome)
    {
        var connectionidDoJogador = Context.ConnectionId;
        var jogador = new Jogador(nome, connectionidDoJogador);

        var novaPartida = new Partida(jogador);

        await _partidaRepository.CriarAsync(novaPartida);

        await Clients.Caller.SendAsync("ReceberCodigoDaPartida", novaPartida.Serializar());
    }

    /// <summary>
    /// Conecta um segundo jogador a uma partida, já criada.
    /// </summary>
    /// <param name="nomeJogador">Nome do jogador</param>
    /// <param name="codPartida">Código da partida existente</param>
    public async Task EntrarPartida(string nomeJogador, string codPartida)
    {
        var partida = await _partidaRepository.ObterPorCodigoAsync(codPartida);

        var partidaJogadorFora = await _partidaRepository.ObterPartidaPorJogadorAsync(Context.ConnectionId);

        if (partida != null)
        {
            var jogadorFora = new Jogador(nomeJogador, Context.ConnectionId);
            partida.ConectarJogadorFora(jogadorFora);
            await _partidaRepository.RemoverAsync(partidaJogadorFora);
            await ComecarPartida(partida);
        }
        else
        {
            await Clients.Caller.SendAsync("AconteceuErro", "Não existe partida com esse código!");
        }
    }

    /// <summary>
    /// Inicializa a partida no lado do cliente, com os dois jogadores na partida.
    /// </summary>
    /// <param name="partida">a partida que está pronta para inicializar</param>
    public async Task ComecarPartida(Partida partida)
    {
        var resposta = partida.Serializar();
        var connectiosIds = new string[] { partida.JogadorLocal.ConnectionId, partida.JogadorFora.ConnectionId };
        await Clients.Clients(connectiosIds).SendAsync("ComecarPartida", resposta);
    }

    /// <summary>
    /// Atribui uma nova marca ao tabuleiro e depois envia uma atualização aos jogadores.
    /// </summary>
    /// <param name="posicao">posicao onde deve ser atribuida a marca</param>
    public async Task MarcarPosicao(string posicao, string partidaSerilizado)
    {
        var partida = JsonConvert.DeserializeObject<Partida>(partidaSerilizado);

        if (!partida.JogadorDaVezConnectionId.Equals(Context.ConnectionId))
            throw new GameException("Não é sua vez");
        var posicoes = posicao.Split(",");
        var vector = new Vector2(float.Parse(posicoes[0]), float.Parse(posicoes[1]));
        
        partida.MarcarPosicao(vector, Context.ConnectionId, this);

        await Clients.Clients(partida.JogadorLocal.ConnectionId, partida.JogadorFora.ConnectionId)
            .SendAsync("AtualizarJogo", partida.Serializar());
    }

    /// <summary>
    /// Declara vitoria em uma partida
    /// </summary>
    /// <param name="partida">Partida onde aconteceu a vitoria</param>
    /// <param name="vencedor">Nome do vencedor</param>
    public async Task FimJogoVitoria(Partida partida , string vencedor)
    {
        await Clients.Clients(partida.JogadorLocal.ConnectionId, partida.JogadorFora.ConnectionId)
            .SendAsync("Vitoria", partida.Serializar(), vencedor);
    }

    /// <summary>
    /// Declara empate em uma partida
    /// </summary>
    /// <param name="partida">Partida onde aconteceu o empate</param>
    public async Task FimJogoEmpate(Partida partida)
    {
        await Clients.Clients(partida.JogadorLocal.ConnectionId, partida.JogadorFora.ConnectionId)
            .SendAsync("Empate", partida.Serializar());
    }

    /// <summary>
    /// Envia um convite para jogar novamente ao seu oponente 
    /// </summary>
    /// <param name="codPartida">codigo da partida no qual o jogador estar</param>
    public async Task JogarNovamente(string codPartida)
    {
        var partida = await _partidaRepository.ObterPorCodigoAsync(codPartida);

        var adversarioConnectionId = partida.JogadorFora.ConnectionId == Context.ConnectionId ?
            partida.JogadorLocal.ConnectionId : partida.JogadorFora.ConnectionId;

        await Clients.Client(adversarioConnectionId).SendAsync("ConviteJogarNovamente");
    }

    /// <summary>
    /// Aceita um convite para jogar novamente
    /// </summary>
    /// <param name="codPartida">codigo da partida no qual o jogador estar</param>
    public async Task JogarNovamenteAceito(string codPartida)
    {
        var partida = await _partidaRepository.ObterPorCodigoAsync(codPartida);
        partida.Resetar();

        await ComecarPartida(partida);
    }

    /// <summary>
    /// Reconecta um jogador que foi desconectado por falha no servidor
    /// </summary>
    /// <param name="connectionIdAntigo">connectionId Antigo do jogador que foi desconectado</param>
    /// <returns></returns>
    public async Task Reconectar(string connectionIdAntigo)
    {
        var partida = await _partidaRepository.ObterPartidaPorJogadorAsync(connectionIdAntigo);
        partida.ReconectarJogador(connectionIdAntigo, Context.ConnectionId);

        await Clients.Clients(partida.JogadorLocal.ConnectionId, partida.JogadorFora.ConnectionId)
            .SendAsync("AtualizarJogo", partida.Serializar());
    }
}
