﻿using Game.Models.Execptions;

namespace Game.Models.Tools;

public class VerificadorTabuleiro
{
    private string posicoesIguais;

    /// <summary>
    /// Verifica se há valores igual nas linhas, colunas e diagonais de uma matriz 3x3.
    /// </summary>
    /// <param name="tabuleiro">Deve ser uma matriz 3x3</param>
    /// <returns>
    /// Retorna true, caso encontre alguma linha, coluna ou diagonal com valores iguais,
    /// caso contrario retorna false.
    /// </returns>
    /// <exception cref="GameException">será lançada uma caso a matriz não seja 3x3</exception>
    public bool Verificar(string[,] tabuleiro)
    {
        if (tabuleiro.GetLength(0) != 3 && tabuleiro.GetLength(1) != 3)
            throw new GameException("Apenas matriz 3x3!");

        Coluna(tabuleiro);
        Linha(tabuleiro);
        Diagonal(tabuleiro);

        if (string.IsNullOrEmpty(posicoesIguais))
            return false;

        return true;
    }


    /// <summary>
    /// Verifica se as colunas do tabuleiro são iguais.
    /// </summary>
    /// <param name="tabuleiro">Deve ser uma matriz 3x3</param>
    /// <remarks>
    /// Atribui a variavel posicoesIgual uma string contendo as posições da coluna que tem valores iguais,
    /// e essa posições estão divida por "|", a string contem 3 posições.
    /// </remarks>
    private void Coluna(string[,] tabuleiro)
    {
        if (tabuleiro[0, 0].Equals(tabuleiro[1, 0]) && tabuleiro[0, 0].Equals(tabuleiro[2, 0]) && tabuleiro[0, 0] != "")
            posicoesIguais = "0,0|1,0|2,0";

        if (tabuleiro[0, 1].Equals(tabuleiro[1, 1]) && tabuleiro[0, 1].Equals(tabuleiro[2, 1]) && tabuleiro[0, 1] != "")
            posicoesIguais = "0,1|1,1|2,1";

        if (tabuleiro[0, 2].Equals(tabuleiro[1, 2]) && tabuleiro[0, 2].Equals(tabuleiro[2, 2]) && tabuleiro[0, 2] != "")
            posicoesIguais = "0,2|1,2|2,2";
    }

    /// <summary>
    /// Verifica se as linhas do tabuleiro são iguais.
    /// </summary>
    /// <param name="tabuleiro">Deve ser uma matriz 3x3</param>
    /// <remarks>
    /// Atribui a variavel posicoesIgual uma string contendo as posições da linha que tem valores iguais,
    /// e essa posições estão divida por "|", a string contem 3 posições.
    /// </remarks>
    private void Linha(string[,] tabuleiro)
    {
        if (tabuleiro[0, 0].Equals(tabuleiro[0, 1]) && tabuleiro[0, 0].Equals(tabuleiro[0, 2]) && tabuleiro[0, 0] != "")
            posicoesIguais = "0,0|0,1|0,2";

        if (tabuleiro[1, 0].Equals(tabuleiro[1, 1]) && tabuleiro[1, 0].Equals(tabuleiro[1, 2]) && tabuleiro[1, 0] != "")
            posicoesIguais = "1,0|1,1|1,2";

        if (tabuleiro[2, 0].Equals(tabuleiro[2, 1]) && tabuleiro[2, 0].Equals(tabuleiro[2, 2]) && tabuleiro[2, 0] != "")
            posicoesIguais = "2,0|2,1|2,2";
    }

    /// <summary>
    /// Verifica se as diagonais do tabuleiro são iguais.
    /// </summary>
    /// <param name="tabuleiro">Deve ser uma matriz 3x3</param>
    /// <remarks>
    /// Atribui a variavel posicoesIgual uma string contendo as posições da diagonal que tem valores iguais,
    /// e essas posições estão divida por "|", a string contem 3 posições.
    /// </remarks>
    private void Diagonal(string[,] tabuleiro)
    {
        if (tabuleiro[0, 0].Equals(tabuleiro[1, 1]) && tabuleiro[0, 0].Equals(tabuleiro[2, 2]) && tabuleiro[0, 0] != "")
            posicoesIguais = "0,0|1,1|2,2";

        if (tabuleiro[2, 0].Equals(tabuleiro[1, 1]) && tabuleiro[2, 0].Equals(tabuleiro[0, 2]) && tabuleiro[2, 0] != "")
            posicoesIguais = "2,0|1,1|0,2";
    }
}