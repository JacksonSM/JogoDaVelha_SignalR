namespace Game.Entity.Tools;

public class VerificadorTabuleiro
{
    /// <summary>
    /// Verifica se as colunas do tabuleiro são iguais.
    /// </summary>
    /// <param name="tabuleiro">Deve ser uma matriz 3x3</param>
    /// <returns>Retorna true, caso os valores de alguma coluna seja igual. Se a matriz não for 3x3, será retornado false</returns>
    public static bool Coluna(string[,] tabuleiro)
    {
        if(tabuleiro.GetLength(0) != 3 && tabuleiro.GetLength(1) != 3)
            return false;

        if (tabuleiro[0,0].Equals(tabuleiro[1,0]) && tabuleiro[0,0].Equals(tabuleiro[2,0]) && tabuleiro[0,0] != "")
            return true;

        if (tabuleiro[0,1].Equals(tabuleiro[1,1]) && tabuleiro[0,1].Equals(tabuleiro[2,1]) && tabuleiro[0,1] != "")
            return true;

        if (tabuleiro[0,2].Equals(tabuleiro[1,2]) && tabuleiro[0,2].Equals(tabuleiro[2,2]) && tabuleiro[0,2] != "")
            return true;

        return false;
    }


    /// <summary>
    /// Verifica se as linhas do tabuleiro são iguais.
    /// </summary>
    /// <param name="tabuleiro">Deve ser uma matriz 3x3</param>
    /// <returns>Retorna true, caso os valores de alguma linha seja igual. Se a matriz não for 3x3, será retornado false</returns>
    public static bool Linha(string[,] tabuleiro)
    {
        if (tabuleiro.GetLength(0) != 3 && tabuleiro.GetLength(1) != 3)
            return false;

        if (tabuleiro[0,0].Equals(tabuleiro[0,1]) && tabuleiro[0, 0].Equals(tabuleiro[0,2]) && tabuleiro[0,0] != "")
            return true;

        if (tabuleiro[1,0].Equals(tabuleiro[1, 1]) && tabuleiro[1,0].Equals(tabuleiro[1,2]) && tabuleiro[1,0] != "")
            return true;

        if (tabuleiro[2,0].Equals(tabuleiro[2,1]) && tabuleiro[2,0].Equals(tabuleiro[2,0]) && tabuleiro[2,0] != "")
            return true;

        return false;
    }
}