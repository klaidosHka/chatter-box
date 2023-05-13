let connection;

$(document).ready(() => {
    connection = new signalR
        .HubConnectionBuilder()
        .withUrl('https://localhost:44340/SignalR/Hub', {
            transport: signalR.HttpTransportType.WebSockets
        })
        .withAutomaticReconnect()
        .build();
    
    connection.start();
});

function getConnection() {
    return connection;
}
