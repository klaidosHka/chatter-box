/* SETUP
 * ------------------------------------------------------------------- */

var connection = new signalR.HubConnectionBuilder()
    .withUrl('https://localhost:44340/SignalR/Hub', {
        transport: signalR.HttpTransportType.WebSockets
    })
    .withAutomaticReconnect()
    .build();

connection.on('ReceiveDirectMessage', (message) => handleDirectMessageReceive(message));
connection.on('ReceiveGroupMessage', (message) => handleGroupMessageReceive(message));

connection
    .start()
    .catch(e => {
        console.error(e);
    });

$("#sendButton").click(function (e) {
    e.preventDefault();

    handleButtonClickSend();
});

/* METHODS
 * ------------------------------------------------------------------- */

function handleButtonClickSend() {
    var input = $("#inputMessage");
    var message = input.val();

    if (message == null || message.trim() === "") {
        input.val(null);

        return;
    }

    handleDirectMessageSend(message);

    //handleGroupMessageSend(message);

    input.val(null);
}

function handleDirectMessageReceive(message) {
    message = reformatText(message);

    $("<li>")
        .addClass("clearfix w-75")
        .append(
            $("<div>")
                .addClass("message my-message")
                .text(message)
        )
        .append(
            $("<div>")
                .addClass("message-data")
                .append(
                    $("<span>")
                        .addClass("message-data-time")
                        .text(new Date().toTimeString())
                )
        )
        .appendTo($("#messages"));
}

function handleDirectMessageSend(message) {
    connection
        .invoke('SendDirectMessage', message)
        .catch(function (e) {
            console.error(e.toString());
        });
}

function handleGroupMessageReceive(message) {
    message = reformatText(message);

    // ...
}

function handleGroupMessageSend(message) {
    connection
        .invoke('SendGroupMessage', message)
        .catch(function (e) {
            console.error(e.toString());
        });
}