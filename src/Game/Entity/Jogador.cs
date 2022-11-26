﻿namespace Game.Entity;

public class Jogador
{
    public string Nome { get; private set; }
    public string ConnectionId { get; private set; }
    public string Marca { get; set; }

    public Jogador(string nome, string connectionId)
    {
        Nome = nome;
        ConnectionId = connectionId;
    }
}