const posicoes = document.getElementsByClassName("posicao");

const btnRevancheModal = document.getElementById("btnRevanche");
const btnNovaPartidaModal = document.getElementById("btnNovaPartida");
const resultadoTextoModal = document.getElementById("resultadoTexto");

const formCriarPartida = document.getElementById("formCriarPartida");
const formEntrarPartida = document.getElementById("formEntrarPartida");

const plJogadorLocal = document.getElementById("plJogadorLocal");
const plJogadorFora = document.getElementById("plJogadorFora");

let connectionIdJogadorDaVez;

let nomeJogador = "";
let nomeJogadorOponente = "";
let codPartida = "";

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
    console.log("entrou!!!!!!");
    const nome = evento.target.elements['fnome'];
    nomeJogador = nome.value;
    connection.invoke("CriarPartida", nome.value);
});

formEntrarPartida.addEventListener("submit", function (evento) {
    evento.preventDefault();

    const cod = evento.target.elements['fcodPardida'];
    connection.invoke("EntrarPartida", nomeJogador, cod.value);
});

btnNovaPartida.addEventListener("click", function (evento) {
    location.reload();
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
    codPartida = partida.CodigoPartida;

    nomeJogadorOponente = partida.JogadorLocal.Nome == nomeJogador
        ? nomeJogadorOponente = partida.JogadorFora.Nome : nomeJogadorOponente = partida.JogadorLocal.Nome;

    atualizarNomeJogadorDaVez();
    resetarTabuleiro();
});

connection.on("AtualizarJogo", (partidaSerializada) => {

    var partida = JSON.parse(partidaSerializada);

    connectionIdJogadorDaVez = partida.JogadorDaVezConnectionId;

    atualizarTabuleiro(partida.Tabuleiro.Posicoes.split(","));
    atualizarNomeJogadorDaVez();
});

connection.on("FimJogo", (partidaSerializada, vencedor) => {
    var partida = JSON.parse(partidaSerializada);

    atualizarTabuleiro(partida.Tabuleiro.Posicoes.split(","));

    var texto = vencedor == nomeJogador ? "Parabéns! você foi mais esperto que seu adversário" :
        "Infelizmente não foi dessa vez :( Peça revanche e a próxima será sua"


    exibirResultadoModal("Vitoria de "+ vencedor +"! ", texto);
});

connection.on("AdversarioDesconectado", () => {
    btnRevancheModal.disabled = false;
    btnRevancheModal.classList.replace("btn-success", "btn-secondary");

    var spanElement = document.createElement("SPAN");
    spanElement.style.color = "#ff0000";
    var text = document.createTextNode(" O seu adversário correu!");
    spanElement.appendChild(text);

    resultadoTextoModal.appendChild(spanElement);
});

function exibirResultadoModal(titulo, texto) {
    var resultadoTituloModal = document.getElementById("resultadoTitulo");
    resultadoTituloModal.innerHTML = titulo;
    resultadoTextoModal.innerHTML = texto;

    $("#resultadoModal").modal({
        show: true
    });
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
    } else {
        alert("Não é sua vez.");
    }
        
};

function atualizarNomeJogadorDaVez() {
    const painelNomeJogador = document.getElementById("nomeJogadorDaVez");

    if (connectionIdJogadorDaVez == connection.connectionId) {
        painelNomeJogador.innerHTML = nomeJogador;
    } else {
        painelNomeJogador.innerHTML = nomeJogadorOponente;
    }


} 


for (let posicao of posicoes) {
    posicao.addEventListener("click", (e) => {
        marcarPosicao(e.target.dataset.posicao);
    });
};

start();
