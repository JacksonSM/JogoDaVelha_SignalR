using Game.Entity.Tools;

namespace Game.Test.Tabuleiro;
public class VerificadorTabuleiroTest
{
    [Fact]
    public void APrimeiraColunaDeveTerValoresIguais()
    {
        var tabuleiro = new string[3, 3] { { "A", "B", "C" }, 
                                           { "A", "E", "F" },
                                           { "A", "H", "I" } };

        var resultado = VerificadorTabuleiro.Coluna(tabuleiro);

        Assert.True(resultado);
    }

    [Fact]
    public void ASegundaColunaDeveTerValoresIguais()
    {
        var tabuleiro = new string[3, 3] { { "A", "B", "C" },
                                           { "K", "B", "F" },
                                           { "P", "B", "I" } };

        var resultado = VerificadorTabuleiro.Coluna(tabuleiro);

        Assert.True(resultado);
    }

    [Fact]
    public void ATerceiraColunaDeveTerValoresIguais()
    {
        var tabuleiro = new string[3, 3] { { "P", "B", "C" },
                                           { "H", "E", "C" },
                                           { "L", "H", "C" } };

        var resultado = VerificadorTabuleiro.Coluna(tabuleiro);

        Assert.True(resultado);
    }

}
