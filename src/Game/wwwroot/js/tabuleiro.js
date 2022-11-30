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
        console.log("marcar posicao");
        console.log(partida);

        let partidaSerilizado = JSON.stringify(partida);
        console.log(partidaSerilizado);
        connection.invoke("MarcarPosicao", posicao, partidaSerilizado);
    }
};


for (let posicao of posicoes) {
    posicao.addEventListener("click", (e) => {
        marcarPosicao(e.target.dataset.posicao);
    });
};