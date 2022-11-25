using Game.Entity.Execptions;
using Game.Entity.Tools;
using System.Numerics;

namespace Game.Entity;

public class Tabuleiro
{
    public string Posicoes { get; set; }

    public delegate string FimDeJogo(string vencedor, string posicoes);

    public Tabuleiro()
    {
        if (Posicoes is null)
            Posicoes = ",,,,,,,,";
    }

    public void MarcarPosicao(string marca, Vector2 posicao)
    {
        ExisteMarca(posicao);

        SetPosicoes(marca, posicao);

        VarificarResultado();
    }

    /// <summary>
    /// Verifica se a posição que deseja marca, já foi marcada.
    /// </summary>
    /// <param name="posicao">A posição que deseja verificar, X = Linha e Y = Coluna</param>
    private void ExisteMarca(Vector2 posicao)
    {
        var posicoesArry = GetPosicoes();
        var FoiMarcado = posicoesArry[(int)posicao.X, (int)posicao.Y] != "";

        if (FoiMarcado) throw new RegrasExceptions("Esta posição já foi marcada!");
    }

    /// <summary>
    /// Esse metodo atribuir ao tabuleiro uma marca, exemplo: "O" ou "X"
    /// </summary>
    /// <param name="marca">A marca que deve ser usada para exibir no tabuleiro.</param>
    /// <param name="posicao">A posição onde deve ser atribuida a marca, X = Linha e Y = Coluna</param>
    private void SetPosicoes(string marca, Vector2 posicao)
    {
        var posicoesArry = GetPosicoes();
        posicoesArry[(int)posicao.X, (int)posicao.Y] = marca;

        Posicoes = string.Join(",", new[] { posicoesArry[0,0], posicoesArry[0,1], posicoesArry[0,2],
                                            posicoesArry[1,0], posicoesArry[1,1], posicoesArry[1,2],
                                            posicoesArry[2,0], posicoesArry[2,1], posicoesArry[2,2]});
    }

    /// <summary>
    /// Para uma melhor manipulação do tabuleiro esse metodo devolve a string tabuleiro em um matriz.
    /// </summary>
    /// <returns>O retorno será o tabuleiro em string para uma matriz 3x3.</returns>
    private string[,] GetPosicoes() {
        var arry = Posicoes.Split(',');

        var posicoesEmMatriz = new string[3, 3] { { arry[0], arry[1], arry[2] },
                                                  { arry[3], arry[4], arry[5] },
                                                  { arry[6], arry[7], arry[8] } };

        return posicoesEmMatriz;
    }

    public void VarificarResultado()
    {
        VerificadorTabuleiro.Linha(GetPosicoes());
        VerificadorTabuleiro.Coluna(GetPosicoes());
    }

}