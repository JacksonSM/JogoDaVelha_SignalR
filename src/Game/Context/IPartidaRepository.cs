using Game.Models;

namespace Game.Context;

public interface IPartidaRepository
{
    Task CriarAsync(Partida novaPartida);
    Task RemoverAsync(Partida partida);
    Task<Partida> ObterPorCodigoAsync(string codPartida);
    Task<Partida> ObterPartidaPorJogadorAsync(string connectionId);
}
