namespace WebApplication_Jogo.Entity;

public class Tabuleiro
{
    public string _posicoes { get;  set; }
    public string[] PosicoesArry 
    {
        get { return _posicoes.Split(","); }
        set { _posicoes = string.Join(",", value);}
    }

    public Tabuleiro()
    {
        PosicoesArry = new string[9];
    }

    public void MarcarPosicao(string marca,int posicao)
    {
        string[] str = new string[]{ "", "", "", "", "", "", "", "", "" };
        str[posicao] = marca;
        _posicoes = string.Join(",", str);
        
       
    }


}