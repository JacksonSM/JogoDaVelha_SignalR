using Game.Entity.Execptions;
using Game.Entity.Tools;
using System.Numerics;

namespace Game.Entity;

public class Tabuleiro
{
    /// <summary>
    /// Posicoes e o campo responsavel por armazenar os valores "X", "O" e "" do tabuleiro.
    /// </summary>
    /// <remarks>
    /// A escolha do tipo foi devido a o banco de dados SQLite que não aceita arry.
    /// </remarks>
    public string Posicoes { get; set; }

    public delegate Task RetaIgual();
    public delegate Task TabuleiroSemReta();

    public Tabuleiro()
    {
        if (Posicoes is null)
                Posicoes = ",,,,,,,,";
    }

    /// <summary>
    /// Método responsável por marcar posição no tabuleiro.
    /// </summary>
    /// <param name="marca">Caractere que será usado para marcar uma posição.</param>
    /// <param name="posicao">Posição na tabela onde a marca será atribuida.</param>
    /// <param name="retaIgual">Método que será invocado caso haja alguma reta com valores iguais.</param>
    /// <param name="posicoesSemReta">
    /// Método que será invocado, caso todas as posições estejam preenchidas e
    /// não haja alguma reta com valores iguais.
    /// </param>
    public void MarcarPosicao( string marca, Vector2 posicao)
    {
        ExisteMarca(posicao);

        SetPosicoes(marca, posicao);
    }

    /// <summary>
    /// Verifica se a posição que deseja marca, já foi marcada.
    /// </summary>
    /// <param name="posicao">A posição que deseja verificar, X = Linha e Y = Coluna</param>
    private void ExisteMarca(Vector2 posicao)
    {
        var posicoesArry = GetPosicoes();
        var FoiMarcado = posicoesArry[(int)posicao.X, (int)posicao.Y] != "";

        if (FoiMarcado) throw new GameExceptions("Esta posição já foi marcada!");
    }

    /// <summary>
    /// Esse método atribui ao tabuleiro uma marca, exemplo: "O" ou "X"
    /// </summary>
    /// <param name="marca">A marca que deve ser usada para exibir no tabuleiro.</param>
    /// <param name="posicao">A posição onde deve ser atribuida a marca, X = Linha e Y = Coluna</param>
    private void SetPosicoes(string marca, Vector2 posicao)
    {
        var tabuleiro = GetPosicoes();
        tabuleiro[(int)posicao.X, (int)posicao.Y] = marca;

        Posicoes = string.Join(",", new[] { tabuleiro[0,0], tabuleiro[0,1], tabuleiro[0,2],
                                            tabuleiro[1,0], tabuleiro[1,1], tabuleiro[1,2],
                                            tabuleiro[2,0], tabuleiro[2,1], tabuleiro[2,2]});
    }

    /// <summary>
    /// Fará uma varredura no tabuleiro em buscar de posições sem marca.
    /// </summary>
    /// <returns>Se encontrar alguma posição vazia retorna falso, caso contrario, true</returns>
    public bool VerificarEmpate()
    {
        var posicoes = Posicoes.Split(',');
        var ExisteEspacoVazio = Array.Exists(posicoes, x => x.Equals(""));
        return !ExisteEspacoVazio;
    }

    /// <summary>
    /// Fará uma varredura no tabuleiro em busca de uma sequência reta de 3 valores iguais,
    /// que seria uma condição de vitoria.
    /// </summary>
    /// <returns>Se encontra uma sequência reta de 3 valores iguais retorna true, caso contrario, false.</returns>
    public bool VerificarVitoria()
    {
        var verificador = new VerificadorTabuleiro();
        var existeRetasIguais = verificador.Verificar(GetPosicoes());

        return existeRetasIguais;
    }

    /// <summary>
    /// Para uma melhor manipulação do tabuleiro esse metodo devolve a string tabuleiro em um matriz.
    /// </summary>
    /// <returns>O retorno será o tabuleiro em string para uma matriz 3x3.</returns>
    private string[,] GetPosicoes() {
        var tabuleiro = Posicoes.Split(',');

        var posicoesEmMatriz = new string[3, 3] { { tabuleiro[0], tabuleiro[1], tabuleiro[2] },
                                                  { tabuleiro[3], tabuleiro[4], tabuleiro[5] },
                                                  { tabuleiro[6], tabuleiro[7], tabuleiro[8] } };

        return posicoesEmMatriz;
    }

    /// <summary>
    /// Deixa todas as posições com a marca vazia.
    /// </summary>
    public void Resetar()
    {
        Posicoes = ",,,,,,,,";
    }
}