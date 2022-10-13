using Microsoft.AspNetCore.SignalR;
using Web.Dados;
using Web.Model;

namespace Web.Hubs;

public class PartidaHub : Hub
{

    private ApplicationDbContext _dbContext;

    public PartidaHub(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
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
}
