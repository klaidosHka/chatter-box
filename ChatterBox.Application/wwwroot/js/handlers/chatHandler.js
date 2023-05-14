$(document).ready(() => {
    getConnection().on('ReceiveMessage', request => handleMessageReceive(request));

    handleChatViewChange(false);

    restructureUsersList();

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

    $(document).on('click', chatElementIds.LISTED_USER, e => {
        handleContextSwitch($(e.currentTarget), chatTypeIds.DIRECT);
    });

    $(chatElementIds.HOME_BUTTON).click(e => {
        handleChatViewChange(false);

        updateUsersForGroup(null);
    });
});

function getContextValues() {
    let contextObject = $(chatElementIds.CONTEXT_DATA);

    return {
        chatType: contextObject.attr(contextAttributes.CHAT_TYPE),
        targetId: contextObject.attr(contextAttributes.TARGET_ID),
        targetName: contextObject.attr(contextAttributes.TARGET_NAME),
        userId: contextObject.attr(contextAttributes.USER_ID),
        userName: contextObject.attr(contextAttributes.USER_NAME)
    }
}

function handleButtonSend() {
    let input = $(chatElementIds.INPUT_TEXT);
    let message = input.val();

    if (message == null || message.trim() === "") {
        input.val(null);

        return;
    }

    handleMessageSend(message);

    input.val(null);
}

function handleContextSwitch(target, chatType) {    
    let id = target.attr(targetAttributes.ID);
    let context = getContextValues();

    if (id === context.targetId) {
        return;
    }

    let name = target.attr(targetAttributes.NAME);

    removeActiveTarget();

    target.addClass("active-target");

    target.attr(targetAttributes.UNREAD, false);

    context.targetId = id;
    context.targetName = name;
    context.chatType = chatType;

    setContextValues(context);

    if (chatType == chatTypeIds.DIRECT) {
        handleUserAddToDirectChat(context.userId, context.targetId);
    } else {
        handleUserAddToGroupChat(context.userId, context.targetId);
    }

    handleChatViewChange(true);
}

function handleChatViewChange(enabled) {
    let context = getContextValues();
    let inputFile = $(chatElementIds.INPUT_FILE).removeClass("green");
    let inputText = $(chatElementIds.INPUT_TEXT).val(null);
    let messages = $(chatElementIds.MESSAGES);
    let targetName = $(chatElementIds.TARGET_NAME);

    image = null;

    if (enabled === true) {
        $(chatElementIds.BUTTON_SEND).fadeIn(300);

        $(chatElementIds.INPUT_EMOTE).fadeIn(300);

        inputText.fadeIn(300);

        inputFile.fadeIn(300);

        targetName.fadeOut(150, () =>
            targetName
                .text(context.targetName)
                .fadeIn(150)
        );

        if (messages.children().length === 0) {
            messages.fadeIn(300, () => fillMessages());
        } else {
            messages.fadeOut(300, () =>
                $(chatElementIds.MESSAGES)
                    .empty()
                    .fadeIn(300, () => fillMessages())
            );
        }

        function fillMessages() {
            let data;
            let url = context.chatType == chatTypeIds.DIRECT
                ? '/Main/Index?handler=Messages'
                : '/Main/Index?handler=GroupMessages'

            if (context.chatType == chatTypeIds.DIRECT) {
                data = {
                    userId: context.userId,
                    targetId: context.targetId
                };
            } else {
                data = {
                    groupId: context.targetId
                };
            }

            $(chatElementIds.MESSAGES_PARENT)
                .removeClass("justify-content-center")
                .addClass("justify-content-start");

            $
                .ajax({
                    url: url,
                    method: 'GET',
                    data: data,
                    dataType: 'json'
                })
                .done(response => {
                    $.each(response, (i, m) => {
                        handleMessageReceive(m);
                    });
                });
        }

        return;
    }

    context.chatType = "";
    context.targetId = "";
    context.targetName = "";

    setContextValues(context);

    removeActiveTarget();

    targetName
        .fadeOut(300, () =>
            targetName
                .text(null)
                .fadeIn(300)
        );

    $(chatElementIds.MESSAGES_PARENT)
        .removeClass("justify-content-start")
        .addClass("justify-content-center");

    messages
        .fadeOut(300, () =>
            messages
                .empty()
                .append(
                    $("<p>")
                        .addClass("fw-bolder")
                        .text("Who are you going to talk to today?")
                )
                .append(
                    $("<p>")
                        .addClass("fw-bolder")
                        .text("Please select an user or a group to begin chatting.")
                )
                .fadeIn(300)
        );

    $(chatElementIds.BUTTON_SEND).fadeOut(300);

    $(chatElementIds.INPUT_EMOTE).fadeOut(300);

    inputText.fadeOut(300);

    inputFile.fadeOut(300);
}

function handleMessageReceive(request) {
    if (!request.signalrId || (!request.receiverId || !request.senderId) && !request.groupId) {
        return;
    }

    let context = getContextValues();

    if (
        !request.groupId &&
        context.chatType == chatTypeIds.DIRECT &&
        request.signalrId !== directChatsByUserId[context.targetId]
    ) {
        let target = getUser(request.senderId);

        target.attr(targetAttributes.UNREAD, true);

        // add unread badge?

        restructureUsersList();

        return;
    }

    request.text = reformatText(request.text);

    let isOwn = context.userId === request.senderId;
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
    let request;
    let formData = new FormData();

    if (context.chatType == chatTypeIds.DIRECT) {
        request = {
            DateSent: new Date(),
            ReceiverId: context.targetId,
            SenderId: context.userId,
            SignalrId: directChatsByUserId[context.targetId],
            Text: message
        };
    } else {
        request = {
            DateSent: new Date(),
            GroupId: context.targetId,
            SenderId: context.userId,
            SignalrId: groupChatsByGroupId[context.targetId],
            Text: message
        };
    }

    if (image) {
        formData.append("file", image);
    }

    formData.append("request", JSON.stringify(request));

    getConnection().invoke(context.chatType == chatTypeIds.DIRECT ? "SendMessage" : "SendGroupMessage", formData);
}

function removeActiveTarget() {
    $(chatElementIds.LISTED_USER).each((i, u) => {
        $(u).removeClass("active-target");
    });

    $(chatElementIds.LISTED_GROUP).each((i, g) => {
        $(g).removeClass("active-target");
    });
}

function restructureUsersList() {
    let users = $(chatElementIds.LISTED_USER);

    users.sort((a, b) => {
        let nameA = $(a).attr(targetAttributes.NAME);
        let nameB = $(b).attr(targetAttributes.NAME);
        let onlineA = $(a).attr(targetAttributes.ONLINE) === 'true';
        let onlineB = $(b).attr(targetAttributes.ONLINE) === 'true';

        if (onlineA && !onlineB) {
            return -1;
        }

        if (!onlineA && onlineB) {
            return 1;
        }

        if (nameA < nameB) {
            return -1;
        }

        if (nameA > nameB) {
            return 1;
        }

        return 0;
    });

    users
        .detach()
        .appendTo($(chatElementIds.USERS_LIST));
}

function setContextValues(context) {
    let contextObject = $(chatElementIds.CONTEXT_DATA);

    contextObject.attr(contextAttributes.CHAT_TYPE, context.chatType);

    contextObject.attr(contextAttributes.TARGET_ID, context.targetId);

    contextObject.attr(contextAttributes.TARGET_NAME, context.targetName);

    contextObject.attr(contextAttributes.USER_ID, context.userId);

    contextObject.attr(contextAttributes.USER_NAME, context.userName);
}

function updateUsersForGroup(groupId) {
    $
        .ajax({
            url: '/Main/Index?handler=GroupUsers',
            method: 'GET',
            data: {
                groupId: groupId
            },
            dataType: 'json'
        })
        .done(response => {
            let users = $(chatElementIds.USERS_LIST);
            let unreadValues = {};

            users.find(chatElementIds.LISTED_USER).each((i, u) => {
                unreadValues[$(u).attr(targetAttributes.ID)] = $(u).attr(targetAttributes.UNREAD);
            });

            users.find(chatElementIds.LISTED_USER).remove();

            $.each(response, (i, u) => {
                if (u.id === $(chatElementIds.CONTEXT_DATA).attr(contextAttributes.USER_ID)) {
                    return;
                }

                let oldUnread = unreadValues[u.id];

                $('<li class="d-flex flex-row context-users-list-user"></li>')
                    .attr('data-own-id', u.id)
                    .attr('data-own-name', u.userName)
                    .attr('data-online', u.online.toString().toLowerCase())
                    .attr('data-unread', oldUnread)
                    .append($('<button class="w-100 d-flex user"></button>')
                        .append($('<img class="rounded-circle avatar ' + (u.online == true ? "avatar-online" : "avatar-offline") + '">')
                            .attr('src', u.avatarLink)
                            .attr('alt', '')
                        )
                        .append($('<div class="d-flex flex-column justify-content-center"></div>')
                            .append($('<div class="justify-content-start"></div>')
                                .text(u.userName))
                        )
                    )
                    .appendTo(users);
            });

            restructureUsersList();
        })
        .fail(xhr => {
            console.log(xhr.status);
        });
}