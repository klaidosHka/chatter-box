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
        
        handleChatTypeChange(getContextObject().chatType);
    
        $(contextIds.BUTTON_SEND).click(e => {
            e.preventDefault();
    
            handleButtonSend();
        });
    
        $(contextIds.INPUT_TEXT).keydown(e => {
            if (e.keyCode !== 13) {
                return;
            }
    
            e.preventDefault();
    
            handleButtonSend();
        });
    }

    function getContextObject() {
        let context = $(contextIds.CONTEXT_DATA);

        return {
            signalRGroupId: context.attr("data-signalr-group-id"),
            userId: context.attr("data-user-id"),
            userTargetId: context.attr("data-target-id"),
            userName: context.attr("data-user-username"),
            chatType: context.attr("data-chat-type")
        }
    }

    function handleButtonSend() {
        let inputMessage = $(contextIds.INPUT_TEXT);
        let message = inputMessage.val();

        if (message == null || message.trim() === "") {
            inputMessage.val(null);

            return;
        }

        handleMessageSend(message);

        inputMessage.val(null);
    }
    
    function handleChatButtonsState(enabled) {
        if (enabled === true) {
            $(contextIds.BUTTON_SEND).prop('disabled', false);
            
            $(contextIds.EMOTE_PICK).prop('disabled', false);
            
            $(contextIds.INPUT_TEXT).prop('disabled', false);
            
            return;
        }
        
        $(contextIds.BUTTON_SEND).prop('disabled', true);
        
        $(contextIds.EMOTE_PICK).prop('disabled', true);
        
        $(contextIds.INPUT_TEXT).prop('disabled', true);
    }

    function handleChatTypeChange(chatType) {
        if (chatType !== chatTypeIds.DIRECT && chatType !== chatTypeIds.GROUP) {
            handleChatButtonsState(false);
            
            return;
        }
        
        handleChatButtonsState(true);
    }

    function handleConnectionStatusChange(userId, isOnline) {
        let context = getContextObject();

        if (context.userId === userId) {
            handleOwnAvatar(userId, isOnline);
        }

        handleListedUserAvatar(userId, isOnline);
    }

    function handleListedUserAvatar(userId, isOnline) {
        let avatarImage = $(contextIds.LISTED_USER + "[data-user-id='" + userId + "']").find("img");

        avatarImage.removeClass("avatar-online avatar-offline");

        avatarImage.addClass(isOnline ? "avatar-online" : "avatar-offline");
    }

    function handleMessageReceive(request) {
        request.text = reformatText(request.text);

        let isOwn = getContextObject().userId === request.id;

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
            .appendTo($("#context-messages"));
    }

    function handleMessageSend(message) {
        let context = getContextObject();

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
