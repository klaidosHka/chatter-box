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

    handleDirectMessageSend(message);

    handleGroupMessageSend(message);

    input.val(null);
}