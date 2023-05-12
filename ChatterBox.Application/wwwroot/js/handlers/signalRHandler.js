let connection;

$(document).ready(() => {
    connection = new signalR
        .HubConnectionBuilder()
        .withUrl('https://localhost:44340/SignalR/Hub', {
            transport: signalR.HttpTransportType.WebSockets
        })
        .withAutomaticReconnect()
        .build();

    connection
        .start()
        .catch(e => {
            console.error(e);
        });

});

function getConnection() {
    return connection;
}
