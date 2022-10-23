namespace WebApplication_Jogo.Entity;

public class Jogador
{
    public string Nome { get; private set; }
    public string ConnectionId { get; private set; }
    public string Marca { get; private set; }

    public Jogador(string nome, string connectionId)
    {
        Nome = nome;
        ConnectionId = connectionId;
    }
}
