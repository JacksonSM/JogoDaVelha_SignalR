﻿using Game.Entity.Tools;

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

    [Fact]
    public void APrimeiraLinhaDeveTerValoresIguais()
    {
        var tabuleiro = new string[3, 3] { { "A", "A", "A" },
                                           { "P", "E", "F" },
                                           { "A", "H", "I" } };

        var resultado = VerificadorTabuleiro.Linha(tabuleiro);

        Assert.True(resultado);
    }

    [Fact]
    public void ASegundaLinhaDeveTerValoresIguais()
    {
        var tabuleiro = new string[3, 3] { { "A", "B", "C" },
                                           { "K", "K", "K" },
                                           { "P", "B", "I" } };

        var resultado = VerificadorTabuleiro.Linha(tabuleiro);

        Assert.True(resultado);
    }

    [Fact]
    public void ATerceiraLinhaDeveTerValoresIguais()
    {
        var tabuleiro = new string[3, 3] { { "P", "B", "C" },
                                           { "K", "E", "C" },
                                           { "L", "L", "L" } };

        var resultado = VerificadorTabuleiro.Linha(tabuleiro);

        Assert.True(resultado);
    }
}
