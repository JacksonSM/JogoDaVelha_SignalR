using Game.DataBase;
using Game.Entity;
using Microsoft.AspNetCore.SignalR;
using WebApplication_Jogo.Entity;

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
        var partida = await _partidaRepository.ObterPartidaPorJogadorLocalAsync(Context.ConnectionId);

        if (partida != null)
        {
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
            await _partidaRepository.ObterPartidaPorJogadorLocalAsync(Context.ConnectionId);

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

        partida.MarcarPosicao(int.Parse(posicao), Context.ConnectionId);

        await _partidaRepository.AtualizarAsync(partida);

        await Clients.Clients(partida.JogadorLocal.ConnectionId, partida.JogadorFora.ConnectionId)
            .SendAsync("AtualizarJogo", partida.Serializar());
    }
}
