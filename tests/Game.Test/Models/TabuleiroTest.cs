using Game.Models;
using Game.Models.Execptions;
using System.Numerics;

namespace Game.Test.Models;
public class TabuleiroTest
{
    [Fact]
    public void TabuleiroDeveEstarComPosicoesVazias()
    {
        Tabuleiro tab = new();

        tab.MarcarPosicao("X", new Vector2(0,0));
        tab.MarcarPosicao("O", new Vector2(0,2));
        tab.MarcarPosicao("X", new Vector2(1,0));
        tab.MarcarPosicao("X", new Vector2(2,0));
        tab.MarcarPosicao("O", new Vector2(2,1));

        tab.Resetar();

        Assert.Equal(",,,,,,,,", tab.Posicoes);
    }

    [Fact]
    public void DeveriaSerVitoriaComValoresIguasEmUmaColuna()
    {
        Tabuleiro tab = new();

        tab.MarcarPosicao("X", new Vector2(0, 0));
        tab.MarcarPosicao("O", new Vector2(0, 2));
        tab.MarcarPosicao("X", new Vector2(1, 0));
        tab.MarcarPosicao("O", new Vector2(1, 1));
        tab.MarcarPosicao("X", new Vector2(2, 0));


        var result = tab.VerificarVitoria();

        Assert.True(result);
    }

    [Fact]
    public void DeveriaSerVitoriaComValoresIguasEmUmaLinha()
    {
        Tabuleiro tab = new();

        tab.MarcarPosicao("X", new Vector2(0, 0));
        tab.MarcarPosicao("O", new Vector2(2, 2));
        tab.MarcarPosicao("X", new Vector2(0, 1));
        tab.MarcarPosicao("O", new Vector2(1, 1));
        tab.MarcarPosicao("X", new Vector2(0, 2));


        var result = tab.VerificarVitoria();

        Assert.True(result);
    }

    [Fact]
    public void DeveriaSerVitoriaComValoresIguasEmUmaDiagonal()
    {
        Tabuleiro tab = new();

        tab.MarcarPosicao("X", new Vector2(0, 0));
        tab.MarcarPosicao("O", new Vector2(0, 2));
        tab.MarcarPosicao("X", new Vector2(1, 1));
        tab.MarcarPosicao("O", new Vector2(1, 0));
        tab.MarcarPosicao("X", new Vector2(2, 2));


        var result = tab.VerificarVitoria();

        Assert.True(result);
    }

    [Fact]
    public void NaoDeveriaDaVitoria()
    {
        Tabuleiro tab = new();

        tab.MarcarPosicao("X", new Vector2(1, 0));
        tab.MarcarPosicao("O", new Vector2(0, 2));
        tab.MarcarPosicao("X", new Vector2(1, 1));
        tab.MarcarPosicao("O", new Vector2(1, 2));
        tab.MarcarPosicao("X", new Vector2(2, 2));


        var result = tab.VerificarVitoria();

        Assert.False(result);
    }

    [Fact]
    public void DeveriaDaEmpate()
    {
        Tabuleiro tab = new();

        tab.MarcarPosicao("X", new Vector2(0, 0));
        tab.MarcarPosicao("O", new Vector2(0, 1));
        tab.MarcarPosicao("X", new Vector2(0, 2));
        tab.MarcarPosicao("O", new Vector2(1, 0));
        tab.MarcarPosicao("X", new Vector2(1, 1));
        tab.MarcarPosicao("O", new Vector2(1, 2));
        tab.MarcarPosicao("X", new Vector2(2, 0));
        tab.MarcarPosicao("O", new Vector2(2, 1));
        tab.MarcarPosicao("X", new Vector2(2, 2));


        var result = tab.VerificarEmpate();

        Assert.True(result);
    }

    [Fact]
    public void NaoDeveriaDaEmpate()
    {
        Tabuleiro tab = new();

        tab.MarcarPosicao("X", new Vector2(0, 0));
        tab.MarcarPosicao("O", new Vector2(0, 1));
        tab.MarcarPosicao("X", new Vector2(0, 2));
        tab.MarcarPosicao("O", new Vector2(1, 0));
        tab.MarcarPosicao("O", new Vector2(1, 2));
        tab.MarcarPosicao("X", new Vector2(2, 0));
        tab.MarcarPosicao("O", new Vector2(2, 1));
        tab.MarcarPosicao("X", new Vector2(2, 2));


        var result = tab.VerificarEmpate();

        Assert.False(result);
    }

    [Fact]
    public void DeveLancarExcecaoQuandoColacaUmaMarcaEmUmaPosicaoOcupada()
    {
        Tabuleiro tab = new();

        tab.MarcarPosicao("X", new Vector2(0, 0));
        tab.MarcarPosicao("O", new Vector2(2, 2));
        tab.MarcarPosicao("X", new Vector2(1, 1));
        tab.MarcarPosicao("O", new Vector2(1, 0));

        var result = tab.VerificarVitoria();

        var ex = Assert.Throws<GameException>(() => tab.MarcarPosicao("X", new Vector2(2, 2)));
        Assert.Equal("Esta posição já foi marcada!", ex.Message);
    }
}
