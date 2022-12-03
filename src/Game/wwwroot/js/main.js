let partida;

let connectionIdJogadorDaVez;

let meuNome = "";
let nomeOponente = "";
let codPartida = "";
let fuiConvidadoParaJogarNovamente = false;

const connection = new signalR.HubConnectionBuilder()
    .withUrl("/jogo-da-velha-hub")
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

connection.on("ReceberCodigoDaPartida", (partidaSerilizado) => {
    partida = JSON.parse(partidaSerilizado);
    proximaTelaParaCodigo();
    exibirCodigoPartida(partida.CodigoPartida);
});

connection.on("ComecarPartida", (partidaSerilizado) => {
    resetarPartida();
    partida = JSON.parse(partidaSerilizado);

    proximaTelaParaPlacar();

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
    partida = JSON.parse(partidaSerializada);

    connectionIdJogadorDaVez = partida.JogadorDaVezConnectionId;

    atualizarTabuleiro(partida.Tabuleiro.Posicoes.split(","));
    atualizarNomeJogadorDaVez();
});

connection.on("Vitoria", (partidaSerializada, vencedor) => {
    partida = JSON.parse(partidaSerializada);

    atualizarTabuleiro(partida.Tabuleiro.Posicoes.split(","));
    resultadoVitoria(vencedor);
});

connection.on("Empate", (partidaSerializada) => {
    partida = JSON.parse(partidaSerializada);

    atualizarTabuleiro(partida.Tabuleiro.Posicoes.split(","));
    resultadoEmpate();
});

connection.on("AdversarioDesconectado", () => {
    adversarioDesconectado();
});

connection.on("ConviteJogarNovamente", () => {
    conviteParaJogarNovamente();
});

connection.on("AconteceuErro", (erro) => {
    alert(erro);
});


function resetarPartida() {
    $('#resultadoModal').modal('hide');
    btnJogarNovamenteModal.disabled = false;
    statusAdversarioModal.textContent = "";
}


start();
