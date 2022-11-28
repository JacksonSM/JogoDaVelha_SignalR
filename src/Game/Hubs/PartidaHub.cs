using Game.DataBase;
using Game.Entity;
using Microsoft.AspNetCore.SignalR;
using System.Numerics;

namespace Game.Hubs;

public class PartidaHub : Hub
{

    private readonly PartidaRepository _partidaRepository;

    public PartidaHub(PartidaRepository partidaRepository)
    {
        _partidaRepository = partidaRepository;
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        var partida = await _partidaRepository.ObterPartidaPorJogadorAsync(Context.ConnectionId);

        if (partida != null)
        {
            var adversarioConnectionId = partida.JogadorLocal.ConnectionId == Context.ConnectionId ? 
                partida.JogadorFora.ConnectionId : partida.JogadorLocal.ConnectionId;

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

        await Clients.Caller.SendAsync("ReceberCodigoDaPartida", novaPartida.CodigoPartida);
    }

    public async Task EntrarPartida(string nomeJogador, string codPartida)
    {
        var partida = await _partidaRepository.ObterPorCodigoAsync(codPartida);

        //remover a partida do banco de dados do jogador que vai entrar na partida
        var partidapRemover =
            await _partidaRepository.ObterPartidaPorJogadorAsync(Context.ConnectionId);

        if (partida != null)
        {
            var jogadorFora = new Jogador(nomeJogador, Context.ConnectionId);
            partida.ConectarJogadorFora(jogadorFora);
        }


        await _partidaRepository.AtualizarAsync(partida);
        await _partidaRepository.RemoverAsync(partidapRemover);
        await ComecarPartida(partida);
    }

    public async Task ComecarPartida(Partida partida)
    {
        var resposta = partida.Serializar();
        var connectiosIds = new string[] { partida.JogadorLocal.ConnectionId, partida.JogadorFora.ConnectionId };
        await Clients.Clients(connectiosIds).SendAsync("ComecarPartida", resposta);
    }

    public async Task MarcarPosicao(string posicao, string codPartida)
    {
        var partida = await _partidaRepository.ObterPorCodigoAsync(codPartida);

        var dividi = posicao.Split(",");
        var posic = new Vector2(float.Parse(dividi[0]), float.Parse(dividi[1]));
        
        partida.MarcarPosicao(posic, Context.ConnectionId, FimJogo);

        await _partidaRepository.AtualizarAsync(partida);

        await Clients.Clients(partida.JogadorLocal.ConnectionId, partida.JogadorFora.ConnectionId)
            .SendAsync("AtualizarJogo", partida.Serializar());
    }

    public async Task FimJogo(Partida partida , string vencedor)
    {
        await Clients.Clients(partida.JogadorLocal.ConnectionId, partida.JogadorFora.ConnectionId)
            .SendAsync("FimJogo", partida.Serializar(), vencedor);
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
        await _partidaRepository.AtualizarAsync(partida);

        await ComecarPartida(partida);
    }
}
