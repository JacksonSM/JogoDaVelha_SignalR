using Game.Context;
using Game.Hubs;
using Game.Models;
using Microsoft.AspNet.SignalR.Hubs;
using Moq;
using System.Numerics;

namespace Game.Test.Models;
public class PartidaTest
{
    [Fact]
    public void ConnectionIdDoJogadorDaVezDeveMudar()
    {
        Jogador local = new("Valdemir", "thrthfg4353");
        Jogador fora = new("Clara", "dfgh6y9df");
        Partida partida = new(local);
        partida.ConectarJogadorFora(fora);


        Vector2 posicao = new Vector2(0,0);
        IMock<IPartidaRepository> repoMock = new Mock<IPartidaRepository>();   
        PartidaHub hub = new PartidaHub(repoMock.Object);

        partida.MarcarPosicao(posicao, "thrthfg4353", hub);

        Assert.Equal("dfgh6y9df", partida.JogadorDaVezConnectionId);
    }

    [Fact]
    public void ValoresDaPartidasDevemEstarResetada()
    {
        Jogador local = new("Cris", "fgrt442");
        Jogador fora = new("Julius", "234gg45");
        Partida partida = new(local);

        partida.Tabuleiro.MarcarPosicao("X", new Vector2(2, 0));
        partida.Tabuleiro.MarcarPosicao("O", new Vector2(0, 2));
        partida.Tabuleiro.MarcarPosicao("X", new Vector2(2, 1));
        partida.Tabuleiro.MarcarPosicao("O", new Vector2(1, 0));


        partida.ConectarJogadorFora(fora);

        partida.Resetar();
        

        Assert.Equal("fgrt442", partida.JogadorDaVezConnectionId);
        Assert.Equal(",,,,,,,,", partida.Tabuleiro.Posicoes);
    }
}
