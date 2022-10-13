const formCriarPartida = document.getElementById("formCriarPartida");
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

// Start the connection.
start();


formCriarPartida.addEventListener("submit", function (evento) {
    evento.preventDefault();

    const nome = evento.target.elements['fnome'];
    console.log(nome.value);
    connection.invoke("CriarPartida", nome.value);
});

connection.on("ReceberCodigoDaPartida", (codigo) => {
    console.log(codigo);
});

