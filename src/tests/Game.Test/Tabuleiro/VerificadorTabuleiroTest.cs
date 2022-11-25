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

        var verificador = new VerificadorTabuleiro();
        var resultado = verificador.Verificar(tabuleiro);

        Assert.Equal(new string[]{"0,0","1,0","2,0"},resultado);
    }

    [Fact]
    public void ASegundaColunaDeveTerValoresIguais()
    {
        var tabuleiro = new string[3, 3] { { "A", "B", "C" },
                                           { "K", "B", "F" },
                                           { "P", "B", "I" } };

        var verificador = new VerificadorTabuleiro();
        var resultado = verificador.Verificar(tabuleiro);

        Assert.Equal(new string[] { "0,1", "1,1", "2,1" }, resultado);
    }

    [Fact]
    public void ATerceiraColunaDeveTerValoresIguais()
    {
        var tabuleiro = new string[3, 3] { { "P", "B", "C" },
                                           { "H", "E", "C" },
                                           { "L", "H", "C" } };

        var verificador = new VerificadorTabuleiro();
        var resultado = verificador.Verificar(tabuleiro);

        Assert.Equal(new string[] { "0,2", "1,2", "2,2" }, resultado);
    }

    [Fact]
    public void APrimeiraLinhaDeveTerValoresIguais()
    {
        var tabuleiro = new string[3, 3] { { "A", "A", "A" },
                                           { "P", "E", "F" },
                                           { "A", "H", "I" } };

        var verificador = new VerificadorTabuleiro();
        var resultado = verificador.Verificar(tabuleiro);

        Assert.Equal(new string[] { "0,0", "0,1", "0,2" }, resultado);
    }

    [Fact]
    public void ASegundaLinhaDeveTerValoresIguais()
    {
        var tabuleiro = new string[3, 3] { { "A", "B", "C" },
                                           { "K", "K", "K" },
                                           { "P", "B", "I" } };

        var verificador = new VerificadorTabuleiro();
        var resultado = verificador.Verificar(tabuleiro);

        Assert.Equal(new string[] { "1,0", "1,1", "1,2" }, resultado);
    }

    [Fact]
    public void ATerceiraLinhaDeveTerValoresIguais()
    {
        var tabuleiro = new string[3, 3] { { "P", "B", "C" },
                                           { "K", "E", "C" },
                                           { "L", "L", "L" } };

        var verificador = new VerificadorTabuleiro();
        var resultado = verificador.Verificar(tabuleiro);

        Assert.Equal(new string[] { "2,0", "2,1", "2,2" }, resultado);
    }

    [Fact]
    public void APrimeiraDiagonalDeveTerValoresIguais()
    {
        var tabuleiro = new string[3, 3] { { "A", "B", "C" },
                                           { "K", "A", "K" },
                                           { "P", "B", "A" } };

        var verificador = new VerificadorTabuleiro();
        var resultado = verificador.Verificar(tabuleiro);

        Assert.Equal(new string[] { "0,0", "1,1", "2,2" }, resultado);
    }

    [Fact]
    public void ASegundaDiagonalDeveTerValoresIguais()
    {
        var tabuleiro = new string[3, 3] { { "P", "B", "L" },
                                           { "K", "L", "C" },
                                           { "L", "G", "E" } };

        var verificador = new VerificadorTabuleiro();
        var resultado = verificador.Verificar(tabuleiro);

        Assert.Equal(new string[] { "2,0", "1,1", "0,2" }, resultado);
    }

}
