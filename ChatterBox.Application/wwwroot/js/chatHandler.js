$("#sendButton").click(function (e) {
    e.preventDefault();

    handleButtonSendClick();
});

function handleButtonSendClick() {
    var input = $("#inputMessage");
    var message = input.val();

    if (message == null || message.trim() === "") {
        input.val(null);

        return;
    }
    handleDirectMessageAdd(message);

    handleGroupMessageAdd(message);

    handleDirectMessageSend(message);

    handleGroupMessageSend(message);

    input.val(null);
}

function handleDirectMessageAdd(message) {
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

function handleGroupMessageAdd(message) {

}