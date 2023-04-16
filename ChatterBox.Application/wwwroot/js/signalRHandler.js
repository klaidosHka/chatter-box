var connection = new signalR.HubConnectionBuilder()
    .withUrl('https://localhost:44340/Main/Index', {
        transport: signalR.HttpTransportType.WebSockets
    })
    .withAutomaticReconnect()
    .build();

connection.on('ReceiveDirectMessage', (message) => handleDirectMessageReceive());
connection.on('ReceiveGroupMessage', (message) => handleGroupMessageReceive());

connection
    .start()
    .catch(e => {
        console.error(e);
    });

function handleDirectMessageReceive(message) {

}

function handleDirectMessageSend(message) {
    connection
        .invoke('SendDirectMessage', message)
        .catch(function (e) {
            // console.error(e.toString());
        });
}

function handleGroupMessageReceive(message) {

}

function handleGroupMessageSend(message) {
    connection
        .invoke('SendGroupMessage', message)
        .catch(function (e) {
            // console.error(e.toString());
        });
}