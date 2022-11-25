using Game.Entity.Execptions;
using Game.Entity.Tools;
using System.Numerics;

namespace Game.Entity;

public class Tabuleiro
{
    public string Posicoes { get; set; }
    public bool Resultado;


    public Tabuleiro()
    {
        if (Posicoes is null)
            Posicoes = ",,,,,,,,";
    }

    public void MarcarPosicao(string marca, Vector2 posicao)
    {
        ExisteMarca(posicao);

        SetPosicoes(marca, posicao);


    }

    private void ExisteMarca(Vector2 posicao)
    {
        var posicoesArry = GetPosicoes();
        var FoiMarcado = posicoesArry[(int)posicao.X, (int)posicao.Y] != "";

        if (FoiMarcado) throw new RegrasExceptions("Esta posição já foi marcada!");
    }

    private void SetPosicoes(string marca, Vector2 posicao)
    {
        var posicoesArry = GetPosicoes();
        posicoesArry[(int)posicao.X, (int)posicao.Y] = marca;

        Posicoes = string.Join(",", new[] { posicoesArry[0,0], posicoesArry[0,1], posicoesArry[0,2],
                                            posicoesArry[1,0], posicoesArry[1,1], posicoesArry[1,2],
                                            posicoesArry[2,0], posicoesArry[2,1], posicoesArry[2,2]});
    }
    private string[,] GetPosicoes() {
        var arry = Posicoes.Split(',');

        var posicoesEmMatriz = new string[3, 3] { { arry[0], arry[1], arry[2] },
                                                  { arry[3], arry[4], arry[5] },
                                                  { arry[6], arry[7], arry[8] } };

        return posicoesEmMatriz;
    }


}