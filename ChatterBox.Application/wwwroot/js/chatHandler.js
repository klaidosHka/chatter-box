$(document).ready(() => {
    var connection;

    executeOnLoad();

    function executeOnLoad() {
        connection = new signalR
            .HubConnectionBuilder()
            .withUrl('https://localhost:44340/SignalR/Hub', {
                transport: signalR.HttpTransportType.WebSockets
            })
            .withAutomaticReconnect()
            .build();

        connection.on('ReceiveMessage', request => handleMessageReceive(request));
        connection.on('OnConnected', userId => handleConnectionStatusChange(userId, true));
        connection.on('OnDisconnected', userId => handleConnectionStatusChange(userId, false));

        connection
            .start()
            .catch(e => {
                console.error(e);
            });

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
            
            if (userId === getContextValues().userTargetId) {
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

            handleChatViewChange(true);
        });
    }

    function getContextObject() {
        return $(chatElementIds.CONTEXT_DATA);
    }

    function getContextValues() {
        let context = getContextObject();

        return {
            chatType: context.attr(contextAttributeIds.CHAT_TYPE),
            signalRGroupId: context.attr(contextAttributeIds.SIGNALR_GROUP_ID),
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
        if (enabled === true) {
            $(chatElementIds.BUTTON_SEND).show();

            $(chatElementIds.EMOTE_PICK).show();

            $(chatElementIds.INPUT_TEXT).show();

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
                    console.log(response);
                    
                    $.each(response, (i, m) => {
                       handleMessageReceive(m);
                    });
                });

            return;
        }

        $(chatElementIds.BUTTON_SEND).hide();

        $(chatElementIds.EMOTE_PICK).hide();

        $(chatElementIds.INPUT_TEXT).hide();
    }

    function handleConnectionStatusChange(userId, isOnline) {
        let context = getContextValues();

        if (context.userId === userId) {
            handleOwnAvatar(userId, isOnline);
        }

        handleListedUserAvatar(userId, isOnline);
    }

    function handleListedUserAvatar(userId, isOnline) {
        let avatarImage = $(chatElementIds.LISTED_USER + "[data-target-id='" + userId + "']").find("img");

        avatarImage.removeClass("avatar-online avatar-offline");

        avatarImage.addClass(isOnline ? "avatar-online" : "avatar-offline");
    }

    function handleMessageReceive(request) {
        request.text = reformatText(request.text);

        let isOwn = getContextValues().userId === request.userId;

        $("<div>")
            .addClass("card w-100 message " + (isOwn ? "my-message" : "other-message"))
            .append(
                $("<div>")
                    .addClass("card-body w-100")
                    .append(
                        $("<p>")
                            .addClass("card-title")
                            .append(
                                $("<span>")
                                    .addClass("fw-bold")
                                    .text(request.userName)
                            )
                            .append(" " + new Date(request.dateSent).toLocaleString('lt-LT', {hour12: false}))
                    )
                    .append(
                        $("<p>")
                            .addClass("card-text")
                            .text(request.text)
                    )
            )
            .appendTo($(chatElementIds.MESSAGES));
    }

    function handleMessageSend(message) {
        let context = getContextValues();

        let request = {
            DateSent: new Date(),
            ReceiverId: context.userTargetId,
            SenderId: context.userId,
            SignalrId: context.signalRGroupId,
            Text: message
        };

        connection
            .invoke(context.chatType === chatTypeIds.GROUP ? "SendGroupMessage" : "SendMessage", request)
            .catch(function (e) {
                console.error(e.toString());
            });
    }

    function handleOwnAvatar(userId, isOnline) {
        let avatarImage = $("#context-own-avatar");

        avatarImage.removeClass("avatar-online avatar-offline");

        avatarImage.addClass(isOnline ? "avatar-online" : "avatar-offline");
    }
});
