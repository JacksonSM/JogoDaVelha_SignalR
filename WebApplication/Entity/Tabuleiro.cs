namespace WebApplication_Jogo.Entity;

public class Tabuleiro
{
    public string[] Posicoes { get; private set; }

    public Tabuleiro()
    {
        Posicoes = new string[9];
    }
}