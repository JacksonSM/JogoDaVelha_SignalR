using Web.Services;

namespace Web.Model;

public class Partida
{
    public int Id { get; set; }

    public Jogador JogadorLocal { get; private set; }
   
    public Jogador JogadorFora { get; private set; }

    public string CodigoPartida { get ; private set; }

    public Partida(){}
    public Partida(Jogador jogadorLocal)
    {
        JogadorLocal = jogadorLocal;
        CodigoPartida = GeradorDeCodigo.Gerar();
    }
}
