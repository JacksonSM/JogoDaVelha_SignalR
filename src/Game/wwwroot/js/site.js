﻿const posicoes = document.getElementsByClassName("posicao");

const btnJogarNovamenteModal = document.getElementById("btnJogarNovamente");
const btnNovaPartidaModal = document.getElementById("btnNovaPartida");
const resultadoTextoModal = document.getElementById("resultadoTexto");
const statusAdversarioModal = document.getElementById("statusAdversarioModal");

const formCriarPartida = document.getElementById("formCriarPartida");
const formEntrarPartida = document.getElementById("formEntrarPartida");

const plJogadorLocal = document.getElementById("plJogadorLocal");
const plJogadorFora = document.getElementById("plJogadorFora");

let connectionIdJogadorDaVez;

let meuNome = "";
let nomeOponente = "";
let codPartida = "";
let fuiConvidadoParaJogarNovamente = false;

const connection = new signalR.HubConnectionBuilder()
    .withUrl("/jogo-da-velha-hub")
    .configureLogging(signalR.LogLevel.Information)
    .build();

async function start() {
    try {
        await connection.start();
    } catch (err) {
        setTimeout(start, 5000);
    }
};

connection.onclose(async () => {
    await start();
});


formCriarPartida.addEventListener("submit", function (evento) {
    evento.preventDefault();
    const nome = evento.target.elements['fnome'];
    meuNome = nome.value;
    connection.invoke("CriarPartida", nome.value);
});

formEntrarPartida.addEventListener("submit", function (evento) {
    evento.preventDefault();

    const cod = evento.target.elements['fcodPardida'];
    connection.invoke("EntrarPartida", meuNome, cod.value);
});

btnNovaPartida.addEventListener("click", function () {
    location.reload();
});

btnJogarNovamenteModal.addEventListener("click", function () {

    if (fuiConvidadoParaJogarNovamente) {
        connection.invoke("JogarNovamenteAceito", codPartida);
        fuiConvidadoParaJogarNovamente = false;
    }
    else
    {
        statusAdversarioModal.style.color = "#28A745";
        statusAdversarioModal.textContent = " Aguardando seu adversário...";

        connection.invoke("JogarNovamente", codPartida);
        btnJogarNovamenteModal.disabled = true;
    }
})

connection.on("ReceberCodigoDaPartida", (codigo) => {

    document.getElementById('formCriarPartida').style.display = 'none';
    const formEntrarPartida = document.getElementById('formEntrarPartida');
    formEntrarPartida.style.display = 'block';

    document.getElementById('codPartida').textContent = codigo;

});

connection.on("ComecarPartida", (partidaSerilizado) => {
    $('#resultadoModal').modal('hide');
    btnJogarNovamenteModal.disabled = false;
    var partida = JSON.parse(partidaSerilizado);
    formEntrarPartida.style.display = "none"

    var painel = document.getElementById("placar");
    painel.style.display = 'block';

    plJogadorLocal.textContent = partida.JogadorLocal.Nome;
    plJogadorFora.textContent = partida.JogadorFora.Nome;
    connectionIdJogadorDaVez = partida.JogadorDaVezConnectionId;
    codPartida = partida.CodigoPartida;

    nomeOponente = partida.JogadorLocal.Nome == meuNome
        ? nomeOponente = partida.JogadorFora.Nome : nomeOponente = partida.JogadorLocal.Nome;

    atualizarNomeJogadorDaVez();
    resetarTabuleiro();
});

connection.on("AtualizarJogo", (partidaSerializada) => {
    var partida = JSON.parse(partidaSerializada);

    connectionIdJogadorDaVez = partida.JogadorDaVezConnectionId;

    atualizarTabuleiro(partida.Tabuleiro.Posicoes.split(","));
    atualizarNomeJogadorDaVez();
});

connection.on("Vitoria", (partidaSerializada, vencedor) => {
    var partida = JSON.parse(partidaSerializada);

    atualizarTabuleiro(partida.Tabuleiro.Posicoes.split(","));
    var titulo;
    var texto;
    titulo = "Vitoria de " + vencedor + "! "
    texto = vencedor == meuNome ? "Parabéns! você foi mais esperto que seu adversário" :
        "Infelizmente não foi dessa vez :( Peça revanche e a próxima será sua"

    exibirResultadoModal(titulo, texto);
});

connection.on("Empate", (partidaSerializada) => {
    var partida = JSON.parse(partidaSerializada);

    atualizarTabuleiro(partida.Tabuleiro.Posicoes.split(","));

    var titulo;
    var texto;
    titulo = "Empate! "
    texto = "Uau! que partida disputada.";

    exibirResultadoModal(titulo, texto);
});

connection.on("AdversarioDesconectado", () => {

    if ($('#resultadoModal').is(':hidden')) {
        alert("O seu adversário saiu!");
        location.reload();
    }
    statusAdversarioModal.style.color = "#ff0000";
    statusAdversarioModal.textContent = "O seu adversário correu!";
    btnJogarNovamenteModal.disabled = true;
});

connection.on("ConviteJogarNovamente", () => {
    statusAdversarioModal.style.color = "#28A745";
    statusAdversarioModal.textContent = "O seu adversário quer jogar novamente";
    fuiConvidadoParaJogarNovamente = true;
});

function exibirResultadoModal(titulo, texto) {
    var resultadoTituloModal = document.getElementById("resultadoTitulo");
    resultadoTituloModal.innerHTML = titulo;
    resultadoTextoModal.innerHTML = texto;


    $('#resultadoModal').modal('show');
}

function atualizarTabuleiro(posicoesArry) {
    for (var i = 0; i < posicoes.length; i++) {
        posicoes[i].innerHTML = posicoesArry[i];
    }
}

function resetarTabuleiro() {

    for (let posicao of posicoes) {
        posicao.innerHTML = "";
    }
};

function marcarPosicao(posicao) {
    if (connectionIdJogadorDaVez === connection.connectionId) {
        connection.invoke("MarcarPosicao", posicao, codPartida);
    } 
};

function atualizarNomeJogadorDaVez() {
    const painelNomeJogador = document.getElementById("nomeJogadorDaVez");

    if (connectionIdJogadorDaVez == connection.connectionId) {
        painelNomeJogador.innerHTML = meuNome;
    } else {
        painelNomeJogador.innerHTML = nomeOponente;
    }


}

for (let posicao of posicoes) {
    posicao.addEventListener("click", (e) => {
        marcarPosicao(e.target.dataset.posicao);
    });
};

start();
