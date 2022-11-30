const posicoes = document.getElementsByClassName("posicao");

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


for (let posicao of posicoes) {
    posicao.addEventListener("click", (e) => {
        marcarPosicao(e.target.dataset.posicao);
    });
};