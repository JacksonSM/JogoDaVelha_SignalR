using Newtonsoft.Json;

namespace Game.Models;

public class Jogador
{
    [JsonProperty]
    public string Nome { get; private set; }
    [JsonProperty]
    public string ConnectionId { get; private set; }
    public string Marca { get; set; }

    public Jogador(string nome, string connectionId)
    {
        Nome = nome;
        ConnectionId = connectionId;
    }
}
