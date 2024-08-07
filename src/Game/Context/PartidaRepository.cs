﻿using Game.Models;
using Microsoft.EntityFrameworkCore;

namespace Game.Context;

public class PartidaRepository : IPartidaRepository
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

    public async Task<Partida> ObterPorCodigoAsync(string codPartida) =>
       await _dbContext.Partidas
            .FirstOrDefaultAsync(x => x.CodigoPartida.Equals(codPartida));

    public async Task<Partida> ObterPartidaPorJogadorAsync(string connectionId) =>
        await _dbContext.Partidas
            .FirstOrDefaultAsync(a => a.JogadorLocal.ConnectionId.Equals(connectionId) ||
                                      a.JogadorFora.ConnectionId.Equals(connectionId));


}
