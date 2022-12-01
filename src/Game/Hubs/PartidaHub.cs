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

    public async Task CriarPartida(string nome)
    {
        var connectionidDoJogador = Context.ConnectionId;
        var jogador = new Jogador(nome, connectionidDoJogador);

        var novaPartida = new Partida(jogador);

        await _partidaRepository.CriarAsync(novaPartida);

        await Clients.Caller.SendAsync("ReceberCodigoDaPartida", novaPartida.CodigoPartida, novaPartida.Serializar());
    }

    public async Task EntrarPartida(string nomeJogador, string codPartida)
    {
        var partida = await _partidaRepository.ObterPorCodigoAsync(codPartida);

        var partidaJogadorFora = await _partidaRepository.ObterPartidaPorJogadorAsync(Context.ConnectionId);

        if (partida != null)
        {
            var jogadorFora = new Jogador(nomeJogador, Context.ConnectionId);
            partida.ConectarJogadorFora(jogadorFora);
        }

        await _partidaRepository.RemoverAsync(partidaJogadorFora);
        await ComecarPartida(partida);
    }

    public async Task ComecarPartida(Partida partida)
    {
        var resposta = partida.Serializar();
        var connectiosIds = new string[] { partida.JogadorLocal.ConnectionId, partida.JogadorFora.ConnectionId };
        await Clients.Clients(connectiosIds).SendAsync("ComecarPartida", resposta);
    }

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
    /// <param name="partida"></param>
    /// <param name="vencedor"></param>
    public async Task FimJogoVitoria(Partida partida , string vencedor)
    {
        await Clients.Clients(partida.JogadorLocal.ConnectionId, partida.JogadorFora.ConnectionId)
            .SendAsync("Vitoria", partida.Serializar(), vencedor);
    }

    public async Task FimJogoEmpate(Partida partida)
    {
        await Clients.Clients(partida.JogadorLocal.ConnectionId, partida.JogadorFora.ConnectionId)
            .SendAsync("Empate", partida.Serializar());
    }

    public async Task JogarNovamente(string codPartida)
    {
        var partida = await _partidaRepository.ObterPorCodigoAsync(codPartida);

        var adversarioConnectionId = partida.JogadorFora.ConnectionId == Context.ConnectionId ?
            partida.JogadorLocal.ConnectionId : partida.JogadorFora.ConnectionId;

        await Clients.Client(adversarioConnectionId).SendAsync("ConviteJogarNovamente");
    }

    public async Task JogarNovamenteAceito(string codPartida)
    {
        var partida = await _partidaRepository.ObterPorCodigoAsync(codPartida);
        partida.Resetar();

        await ComecarPartida(partida);
    }
}
