using Microsoft.AspNetCore.SignalR;
using WebApplication_Jogo.DataBase;
using WebApplication_Jogo.Entity;

namespace WebApplication_Jogo.Hubs;

public class PartidaHub : Hub
{

    private ApplicationDbContext _dbContext;

    public PartidaHub(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        var partida = _dbContext.Partidas
            .FirstOrDefault(a => a.JogadorLocal.ConnectionId.Contains(Context.ConnectionId));

        if (partida != null)
        {
            _dbContext.Remove(partida);
            _dbContext.SaveChanges();
        }

        await base.OnDisconnectedAsync(exception);
    }
    public async Task CriarPartida(string nome)
    {
        var connectionidDoJogador = Context.ConnectionId;
        var jogador = new Jogador(nome, connectionidDoJogador);

        var novaPartida = new Partida(jogador);

        _dbContext.Partidas.Add(novaPartida);
        await _dbContext.SaveChangesAsync();

        await Clients.Caller.SendAsync("ReceberCodigoDaPartida", novaPartida.CodigoPartida);
    }
    public async Task EntrarPartida(string nomeJogador, string codPartida)
    {
        var partida = _dbContext.Partidas
            .FirstOrDefault(x => x.CodigoPartida.Equals(codPartida));

        //remover a partida do banco de dados do jogador que vai entrar na partida
        var partidapRemover = _dbContext.Partidas
            .FirstOrDefault(x => x.JogadorLocal.ConnectionId.Equals(Context.ConnectionId));

        if (partida != null)
        {
            var jogadorFora = new Jogador(nomeJogador, Context.ConnectionId);
            partida.ConectarJogadorFora(jogadorFora);
        }

        _dbContext.Partidas.Update(partida);
        _dbContext.Partidas.Remove(partidapRemover);
        await _dbContext.SaveChangesAsync();

        await ComecarPartida(partida);

    }
    public async Task ComecarPartida(Partida partida)
    {
        var resposta = partida.Serializar();
        var connectiosIds = new string[]{partida.JogadorLocal.ConnectionId,partida.JogadorFora.ConnectionId };
        //TODO - testar
        await Clients.Clients(connectiosIds).SendAsync("ComecarPartida",resposta);
    }
}
