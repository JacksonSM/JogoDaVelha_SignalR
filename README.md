# Jogo da Velha - Multiplayer Online

![Jogo da velha](https://user-images.githubusercontent.com/90290547/205185139-92ed52c4-805c-48ac-82f4-ec4b1daa5d29.PNG)

## Deploy

Aplicação foi hospedado na plataforma Azure</br>
Link para a aplicação: https://jogo-da-velha-jackson-sm.azurewebsites.net/

## Sobre

A motivação para este projeto foi apenas curiosidade de como seria desenvolver um jogo multiplayer em tempo real, já tinha visto algo sobre o SignalR
e sabia que este framework seria o ideal. Quando eu pude, decidi desenvolver. Confesso que o maior desafio não foi nem usar um framework para aplicação de comunicação em TEMPO REAL, mas sim o HTML e CSS, Eu e o front não nos damos bem. :smile:

## Implementação SignalR
Pode parece complicado desenvolver uma aplicação com a comunicação em tempo real, mas não com o SignalR, pois, 
este framework abstrai bem a tecnologia fornecendo ao desenvolvedor métodos bem intuitivos para fazer a comunicação.

### Configuração inicial
A configuração é bem simples, no `Program.cs`:

adicionei o serviço do SignalR, no meu caso resolvi criar um filtro para capturar as exceções.
````
builder.Services.AddSignalR(options =>
{
    options.AddFilter<ExceptionFilter>();
});
````

Ainda no `Program.cs` declaramos a rota para o cliente ter acesso, veja que existe um método com um tipo `PartidaHuB`,
esta é uma classe que herda de Hub, e é nesta classe que foi criado toda a lógica de comunicação entres os clientes.

````
app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<PartidaHub>("/jogo-da-velha-hub");
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});
````

Já no lado do cliente, foi adicionado o seguinte código em JavaScript: 
````
//Instanciando uma conexão ao meu hub
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/jogo-da-velha-hub") //veja que essa Url que estou passando como parâmetro foi o que eu declarei no Program.cs
    .build();

//nesta função é iniciado a conexão, caso falhe, após 5 segundos tentará novamente.
async function start() {
    try {
        await connection.start();
    } catch (err) {
        setTimeout(start, 5000);
    }
};

//Para caso a conexão caia
connection.onclose(async () => {
    await start();
});

start();
````
Apenas com isso o cliente e o servidor já estão conectados.

### Comunicação

O SignalR nos fornece uma API para criar RPC (chamadas de procedimento remoto), 
a comunicação entre cliente e servidor é bem simples: basicamente um método do lado do cliente invoca um método do lado do servidor e vice-versa.
Para demostrar isso, usarei como exemplo o que foi feito para criar uma partida:

Do lado do cliente usamos o metodo "invoke" para invocar um método no servidor.

**Cliente:** 

No bloco de codigo abaixo, foi capturado o nome do jogador, logo após, foi chamado o invoke passando como parâmetros, 
primeiro o nome da função que está no sevidor, e o seus parâmetros que no caso seria apenas o nome do jogador. Para funcionar é importante que no invoke
seja passado os dados seguindo a assinatura do método que está no servidor de forma correta.
````
formCriarPartida.addEventListener("submit", function (evento) {
    evento.preventDefault();
    const nome = evento.target.elements['fnome'];
    connection.invoke("CriarPartida", nome.value);
});
````

**Servidor:**

Na classe que herda de Hub e que foi usada para criação da rota, a `PartidaHub.cs`, foi escrito o seguinte método:
````
public async Task CriarPartida(string nome)
{
    var connectionidDoJogador = Context.ConnectionId; // Context.ConnectionId retorna a connectionId do usuario que invocou o metodo.
    var jogador = new Jogador(nome, connectionidDoJogador);

    var novaPartida = new Partida(jogador);

    await _partidaRepository.CriarAsync(novaPartida);

    await Clients.Caller.SendAsync("ReceberCodigoDaPartida", novaPartida.Serializar());
}
````
No código da última linha, está invocando um método chamado "ReceberCodigoDaPartida" ao lado do cliente, passando como parâmetro
o objeto partida serializada.

**Cliente:** 

Então o cliente recebe a partida serializada, desserializa, e exibi o código que está no objeto partida.
````
connection.on("ReceberCodigoDaPartida", (partidaSerilizado) => {
    partida = JSON.parse(partidaSerilizado);
    proximaTelaParaCodigo();
    exibirCodigoPartida(partida.CodigoPartida);
});
````
Tudo isso acontecendo de forma quase instantânea.

## Para rodar o projeto em sua máquina local
* Faça o clone do repositório `git clone https://github.com/JacksonSM/JogoDaVelha_SignalR.git`
* Precisa ter instalado o .NET 6 em sua máquina. 

### No Visual Studio 2022
* Definir o projeto Game como projeto de inicialização.
* Agora é só rodar.

### No Visual Studio Code
* Vá até a pasta JogoDaVelha_SignalR\src\Game
* Digite o seguinte código no terminal: `dotnet run --project Game.csproj`
* Acesse a aplicação por meio desse link: https://localhost:7190/

Para saber mais: [Introdução ao SignalR](https://learn.microsoft.com/pt-br/aspnet/signalr/overview/getting-started/introduction-to-signalr)
