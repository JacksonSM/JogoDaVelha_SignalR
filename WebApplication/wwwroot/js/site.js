const posicoes = document.getElementsByClassName("posicao");
let tabuleiro = ["", "", "", "", "", "", "", "", ""];


const formCriarPartida = document.getElementById("formCriarPartida");
const formEntrarPartida = document.getElementById("formEntrarPartida");

const plJogadorLocal = document.getElementById("plJogadorLocal");
const plJogadorFora = document.getElementById("plJogadorFora");

let connectionIdJogadorDaVez;

let nomeJogador = "";
let nomeJogadorOponente = "";

const connection = new signalR.HubConnectionBuilder()
    .withUrl("/jogo-da-velha-hub")
    .configureLogging(signalR.LogLevel.Information)
    .build();

async function start() {
    try {
        await connection.start();
        console.log("SignalR Connected.");
    } catch (err) {
        console.log(err);
        setTimeout(start, 5000);
    }
};

connection.onclose(async () => {
    await start();
});


formCriarPartida.addEventListener("submit", function (evento) {
    evento.preventDefault();

    const nome = evento.target.elements['fnome'];
    nomeJogador = nome.value;
    connection.invoke("CriarPartida", nome.value);
});

formEntrarPartida.addEventListener("submit", function (evento) {
    evento.preventDefault();

    const cod = evento.target.elements['fcodPardida'];
    connection.invoke("EntrarPartida", nomeJogador, cod.value);
});

connection.on("ReceberCodigoDaPartida", (codigo) => {

    document.getElementById('formCriarPartida').style.display = 'none';
    const formEntrarPartida = document.getElementById('formEntrarPartida');
    formEntrarPartida.style.display = 'block';

    document.getElementById('codPartida').textContent = codigo;

});

connection.on("ComecarPartida", (partidaSerilizado) => {

    var partida = JSON.parse(partidaSerilizado);
    formEntrarPartida.style.display = "none"

    var painel = document.getElementById("placar");
    painel.style.display = 'block';

    plJogadorLocal.textContent = partida.JogadorLocal.Nome;
    plJogadorFora.textContent = partida.JogadorFora.Nome;
    connectionIdJogadorDaVez = partida.JogadorDaVezConnectionId;

    nomeJogadorOponente = partida.JogadorLocal.Nome == nomeJogador
        ? nomeJogadorOponente = partida.JogadorFora.Nome : nomeJogadorOponente = partida.JogadorLocal.Nome;

    atualizarNomeJogadorDaVez();
    resetarTabuleiro();
});

function resetarTabuleiro() {

    for (let posicao of posicoes) {
        posicao.innerHTML = "";
    }
};


for (let posicao of posicoes) {
    posicao.addEventListener("click", (e) => {
        marcarPosicao("X", e.target);
    });
};

function marcarPosicao(marca, posicao) {

    if (connectionIdJogadorDaVez === connection.connectionId) {
        posicao.innerHTML = marca;
    } else {
        alert("Não é sua vez.");
    }
        
};

function atualizarNomeJogadorDaVez() {
    const painelNomeJogador = document.getElementById("nomeJogadorDaVez");

    console.log(connectionIdJogadorDaVez + "---------" + connection.connectionId);

    if (connectionIdJogadorDaVez == connection.connectionId) {
        painelNomeJogador.innerHTML = nomeJogador;
    } else {
        painelNomeJogador.innerHTML = nomeJogadorOponente;
    }


} 



start();
