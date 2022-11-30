const btnJogarNovamenteModal = document.getElementById("btnJogarNovamente");
const btnNovaPartidaModal = document.getElementById("btnNovaPartida");
const resultadoTextoModal = document.getElementById("resultadoTexto");
const statusAdversarioModal = document.getElementById("statusAdversarioModal");

btnNovaPartida.addEventListener("click", function () {
    location.reload();
});

btnJogarNovamenteModal.addEventListener("click", function () {

    if (fuiConvidadoParaJogarNovamente) {
        connection.invoke("JogarNovamenteAceito", codPartida);
        fuiConvidadoParaJogarNovamente = false;
    }
    else {
        statusAdversarioModal.style.color = "#28A745";
        statusAdversarioModal.textContent = " Aguardando seu adversário...";

        connection.invoke("JogarNovamente", codPartida);
        btnJogarNovamenteModal.disabled = true;
    }
});

function resultadoVitoria(vencedor) {
    var titulo;
    var texto;
    titulo = "Vitoria de " + vencedor + "! "
    texto = vencedor == meuNome ? "Parabéns! você foi mais esperto que seu adversário" :
        "Infelizmente não foi dessa vez :( Peça revanche e a próxima será sua"

    exibirResultadoModal(titulo, texto);
}

function resultadoEmpate() {
    var titulo;
    var texto;
    titulo = "Empate! "
    texto = "Uau! que partida disputada.";

    exibirResultadoModal(titulo, texto);
}

function conviteParaJogarNovamente() {
    statusAdversarioModal.style.color = "#28A745";
    statusAdversarioModal.textContent = "O seu adversário quer jogar novamente";
    fuiConvidadoParaJogarNovamente = true;
}

function adversarioDesconectado() {
    if ($('#resultadoModal').is(':hidden')) {
        alert("O seu adversário saiu!");
        location.reload();
    }

    statusAdversarioModal.style.color = "#ff0000";
    statusAdversarioModal.textContent = "O seu adversário correu!";
    btnJogarNovamenteModal.disabled = true;
}

function exibirResultadoModal(titulo, texto) {
    var resultadoTituloModal = document.getElementById("resultadoTitulo");
    resultadoTituloModal.innerHTML = titulo;
    resultadoTextoModal.innerHTML = texto;


    $('#resultadoModal').modal('show');
}
