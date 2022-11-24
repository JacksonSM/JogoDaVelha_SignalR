using Game.Entity.Tools;
using WebApplication_Jogo.Entity.Execptions;

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

    public void MarcarPosicao(string marca, int posicao)
    {
        ExisteMarca(posicao);

        SetPosicoes(marca, posicao);

        VarificarResultado();
    }

    private void ExisteMarca(int posicao)
    {
        var posicoesArry = GetPosicoes();
        var FoiMarcado = posicoesArry[posicao] != "";

        if (FoiMarcado) throw new RegrasExceptions("Esta posição já foi marcada!");
    }

    private void SetPosicoes(string marca, int posicao)
    {
        var posicoesArry = GetPosicoes();
        posicoesArry[posicao] = marca;
        Posicoes = string.Join(",", posicoesArry);
    }
    private string[] GetPosicoes() =>
        Posicoes.Split(',');

    public void VarificarResultado()
    {
        VerificadorTabuleiro.LinhaVertical(GetPosicoes());
    }
}