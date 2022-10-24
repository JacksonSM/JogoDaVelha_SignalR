using Microsoft.EntityFrameworkCore;
using WebApplication_Jogo.Entity;

namespace WebApplication_Jogo.DataBase;

public class PartidaRepository
{
    private ApplicationDbContext _dbContext;

    public PartidaRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CriarAsync(Partida novaPartida)
    {
        _dbContext.Partidas.Add(novaPartida);
        await _dbContext.SaveChangesAsync();
    }

    public async Task RemoverAsync(Partida partida)
    {
        _dbContext.Remove(partida);
        await _dbContext.SaveChangesAsync();
    }
    public async Task AtualizarAsync(Partida partida)
    {
        _dbContext.Update(partida);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Partida> ObterPorCodigoAsync(string codPartida) =>
       await _dbContext.Partidas
            .FirstOrDefaultAsync(x => x.CodigoPartida.Equals(codPartida));

    public async Task<Partida> ObterPartidaPorJogadorLocalAsync(string connectionId) =>
        await _dbContext.Partidas
            .FirstOrDefaultAsync(a => a.JogadorLocal.ConnectionId.Contains(connectionId));


}
