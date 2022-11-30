const formCriarPartida = document.getElementById("formCriarPartida");
const formEntrarPartida = document.getElementById("formEntrarPartida");

const plJogadorLocal = document.getElementById("plJogadorLocal");
const plJogadorFora = document.getElementById("plJogadorFora");

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

function atualizarNomeJogadorDaVez() {
    const painelNomeJogador = document.getElementById("nomeJogadorDaVez");

    if (connectionIdJogadorDaVez == connection.connectionId) {
        painelNomeJogador.innerHTML = meuNome;
    } else {
        painelNomeJogador.innerHTML = nomeOponente;
    }
}

function proximaTelaParaCodigo() {
    document.getElementById('formCriarPartida').style.display = 'none';
    const formEntrarPartida = document.getElementById('formEntrarPartida');
    formEntrarPartida.style.display = 'block';
}

function exibirCodigoPartida(codigo) {
    document.getElementById('codPartida').textContent = codigo;
}

function proximaTelaParaPlacar() {
    formEntrarPartida.style.display = "none"
    var painel = document.getElementById("placar");
    painel.style.display = 'block';
}