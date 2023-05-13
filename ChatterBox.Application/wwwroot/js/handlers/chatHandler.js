$(document).ready(() => {
    getConnection().on('ReceiveMessage', request => handleMessageReceive(request));

    handleChatViewChange(false);

    $(chatElementIds.BUTTON_SEND).click(e => {
        e.preventDefault();

        handleButtonSend();
    });

    $(chatElementIds.INPUT_TEXT).keydown(e => {
        if (e.keyCode !== 13) {
            return;
        }

        e.preventDefault();

        handleButtonSend();
    });

    $(chatElementIds.LISTED_USER).click(e => {
        let user = $(e.currentTarget);
        let userId = user.attr(contextAttributeIds.USER_TARGET_ID);
        let contextValues = getContextValues();

        if (userId === contextValues.userTargetId) {
            return;
        }

        let userName = user.attr(contextAttributeIds.USER_TARGET_NAME);

        $(chatElementIds.LISTED_USER).each((i, u) => {
            $(u).removeClass("user-active-target");
        });

        user.addClass("user-active-target");

        let context = getContextObject();

        context.attr(contextAttributeIds.USER_TARGET_ID, userId);

        context.attr(contextAttributeIds.USER_TARGET_NAME, userName);

        context.attr(contextAttributeIds.CHAT_TYPE, chatTypeIds.DIRECT);

        handleUserAddToDirectChat(contextValues.userId, userId);

        handleChatViewChange(true);
    });
});

function getContextObject() {
    return $(chatElementIds.CONTEXT_DATA);
}

function getContextValues() {
    let context = getContextObject();

    return {
        chatType: context.attr(contextAttributeIds.CHAT_TYPE),
        userId: context.attr(contextAttributeIds.USER_ID),
        userName: context.attr(contextAttributeIds.USER_NAME),
        userTargetId: context.attr(contextAttributeIds.USER_TARGET_ID),
        userTargetName: context.attr(contextAttributeIds.USER_TARGET_NAME)
    }
}

function handleButtonSend() {
    let inputMessage = $(chatElementIds.INPUT_TEXT);
    let message = inputMessage.val();

    if (message == null || message.trim() === "") {
        inputMessage.val(null);

        return;
    }

    handleMessageSend(message);

    inputMessage.val(null);
}

function handleChatViewChange(enabled) {
    let inputFile = $(chatElementIds.INPUT_FILE).removeClass("green");

    image = null;

    if (enabled === true) {
        $(chatElementIds.BUTTON_SEND).show();

        $(chatElementIds.INPUT_EMOTE).show();

        $(chatElementIds.INPUT_TEXT).show();

        inputFile.show();

        $(chatElementIds.TARGET_NAME).text(getContextValues().userTargetName)

        let messagesParent = $(chatElementIds.MESSAGES_PARENT);

        messagesParent.removeClass("justify-content-center");

        messagesParent.addClass("justify-content-start");

        $(chatElementIds.MESSAGES).empty();

        let context = getContextValues();

        $
            .ajax({
                url: '/Main/Index?handler=Messages',
                method: 'GET',
                data: {
                    userId: context.userId,
                    targetId: context.userTargetId
                },
                dataType: 'json'
            })
            .done(response => {
                $.each(response, (i, m) => {
                    handleMessageReceive(m);
                });
            });

        return;
    }

    $(chatElementIds.BUTTON_SEND).hide();

    $(chatElementIds.INPUT_EMOTE).hide();

    $(chatElementIds.INPUT_TEXT).hide();

    inputFile.hide();
}

function handleMessageReceive(request) {      
    if (!request.signalrId || !request.receiverId || !request.senderId) {
        return;
    }
    
    if (request.signalrId !== directChatsByUserId[getContextValues().userTargetId]) {
        // add unread notification next to the [@senderId] user in the list
        
        return;
    }
    
    request.text = reformatText(request.text);

    let isOwn = getContextValues().userId === request.senderId;
    let messages = $(chatElementIds.MESSAGES);

    $("<div>")
        .addClass("card w-100 message " + (isOwn ? "my-message" : "other-message"))
        .append(
            $("<div>")
                .addClass("card-body w-100")
                .append(
                    $("<div>")
                        .addClass("d-flex card-title row justify-content-between align-items-center")
                        .append(
                            $("<span>")
                                .addClass("d-flex fw-bold w-50 justify-content-start align-items-center")
                                .text(request.senderUserName + (isOwn ? " (You)" : ""))
                        )
                        .append(
                            $("<div>")
                                .addClass("d-flex w-50 justify-content-end align-items-center")
                                .append(" " + new Date(request.dateSent).toLocaleString('lt-LT', {hour12: false}))
                        )
                )
                .append(
                    $("<div>")
                        .addClass("card-text")
                        .text(request.text)
                )
        )
        .appendTo(messages);

    $(".chat-middle")
        .find(chatElementIds.MESSAGES)
        .children(':last-child')[0]
        .scrollIntoView();
}

function handleMessageSend(message) {
    let context = getContextValues();

    let request = {
        DateSent: new Date(),
        ReceiverId: context.userTargetId,
        SenderId: context.userId,
        SignalrId: directChatsByUserId[context.userTargetId],
        Text: message
    };

    getConnection()
        .invoke(context.chatType === chatTypeIds.GROUP ? "SendGroupMessage" : "SendMessage", request)
        .catch(e => {
            console.error(e.toString());
        });
}
