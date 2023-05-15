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
        if (getContextValues().chatType == chatTypeIds.GROUP) {
            return;
        }

        handleContextSwitch($(e.currentTarget), chatTypeIds.DIRECT);
    });

    $(chatElementIds.HOME_BUTTON).click(e => {
        updateUsersForGroup(null);

        handleChatViewChange(false);
    });
});

function getContextValues() {
    let contextObject = $(chatElementIds.CONTEXT_DATA);

    return {
        chatType: contextObject.attr(contextAttributes.CHAT_TYPE),
        targetId: contextObject.attr(contextAttributes.TARGET_ID),
        targetOwnerId: contextObject.attr(contextAttributes.TARGET_OWNER_ID),
        targetName: contextObject.attr(contextAttributes.TARGET_NAME),
        userId: contextObject.attr(contextAttributes.USER_ID),
        userName: contextObject.attr(contextAttributes.USER_NAME)
    }
}

function handleButtonSend() {
    let input = $(chatElementIds.INPUT_TEXT);
    let message = input.val();

    if ((message == null || message.trim() === "") && !image) {
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

    if (chatType == chatTypeIds.GROUP) {
        context.targetOwnerId = target.attr(targetAttributes.OWNER_ID);
    }

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

    if (context.chatType == chatTypeIds.GROUP) {
        $(chatElementIds.GROUP_MORE).fadeIn(150);
    }

    let inputFile = $(chatElementIds.INPUT_FILE).removeClass("green");
    let inputText = $(chatElementIds.INPUT_TEXT).val(null);
    let messages = $(chatElementIds.MESSAGES);
    let targetName = $(chatElementIds.TARGET_NAME);

    $(chatElementIds.LISTED_USER).each((i, u) => $(u).removeClass("d-none"));

    $(chatElementIds.SEARCH).val(null);

    image = null;

    if (enabled === true) {
        $(chatElementIds.BUTTON_SEND).fadeIn(150);

        $(chatElementIds.INPUT_EMOTE).fadeIn(150);

        inputText.fadeIn(150);

        inputFile.fadeIn(150);

        targetName.fadeOut(75, () =>
            targetName
                .text(context.targetName)
                .fadeIn(75)
        );

        if (messages.children().length === 0) {
            messages.fadeIn(75, () => fillMessages());
        } else {
            messages.fadeOut(75, () =>
                $(chatElementIds.MESSAGES)
                    .empty()
                    .fadeIn(75, () => fillMessages())
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
    context.targetOwnerId = "";
    context.targetName = "";

    setContextValues(context);

    removeActiveTarget();

    targetName.fadeOut(75, () =>
        targetName
            .text(null)
            .fadeIn(75)
    );

    $(chatElementIds.GROUP_MORE).fadeOut(150);

    $(chatElementIds.MESSAGES_PARENT)
        .removeClass("justify-content-start")
        .addClass("justify-content-center");

    messages.fadeOut(150, () =>
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
            .fadeIn(150)
    );

    $(chatElementIds.BUTTON_SEND).fadeOut(150);

    $(chatElementIds.INPUT_EMOTE).fadeOut(150);

    inputText.fadeOut(150);

    inputFile.fadeOut(150);
}

function handleMessageReceive(request) {
    let context = getContextValues();

    if (
        !request.signalrId ||
        (!request.receiverId || !request.senderId) && !request.groupId ||
        request.groupId && context.chatType != chatTypeIds.GROUP ||
        !request.groupId && context.chatType != chatTypeIds.DIRECT
    ) {
        return;
    }
    
    if (
        !request.groupId &&
        context.chatType == chatTypeIds.DIRECT &&
        request.signalrId !== directChatsByUserId[context.targetId]
    ) {
        let user = getUser(request.senderId);

        user.attr(targetAttributes.UNREAD, true);

        restructureUsersList();

        playNotificationSound();

        return;
    }
    
    if (
        request.groupId &&
        context.chatType == chatTypeIds.GROUP &&
        request.signalrId !== groupChatsByGroupId[context.targetId]
    ) {
        let group = getGroup(request.groupId);

        group.attr(targetAttributes.UNREAD, true);
        
        playNotificationSound();
        
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
                        .addClass("card-text justify-text")
                        .text(request.text)
                )
                .append(
                    request.imageLink
                        ? $("<img class='message-img card-text rounded' style='height: 250px' alt='' src='" + request.imageLink + "'>")
                        : null
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

    if (context.chatType == chatTypeIds.DIRECT) {
        request = {
            DateSent: new Date(),
            FileBytes: null,
            FileName: null,
            ReceiverId: context.targetId,
            SenderId: context.userId,
            SignalrId: directChatsByUserId[context.targetId],
            Text: message
        };
    } else {
        request = {
            DateSent: new Date(),
            FileBytes: null,
            FileName: null,
            GroupId: context.targetId,
            SenderId: context.userId,
            SignalrId: groupChatsByGroupId[context.targetId],
            Text: message
        };
    }

    if (image) {
        request.FileName = image.name;

        let reader = new FileReader();

        reader.onload = function (event) {
            request.FileBytes = event.target.result.split(',')[1];

            getConnection()
                .invoke(context.chatType == chatTypeIds.DIRECT ? "SendMessage" : "SendGroupMessage", request)
                .catch(e => {
                    console.log(e);
                });
            
            image = null;
        };
        
        reader.readAsDataURL(image);
        
        $(chatElementIds.INPUT_FILE).removeClass("green");
    } else {
        getConnection().invoke(context.chatType == chatTypeIds.DIRECT ? "SendMessage" : "SendGroupMessage", request);
    }
}

function playNotificationSound() {
    new Audio("/sounds/sound1.wav").play();
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

    contextObject.attr(contextAttributes.TARGET_OWNER_ID, context.targetOwnerId);

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

            users
                .find(chatElementIds.LISTED_USER)
                .each((i, u) => {
                    unreadValues[$(u).attr(targetAttributes.ID)] = $(u).attr(targetAttributes.UNREAD);
                });

            users.fadeOut(150, () => {
                users
                    .find(chatElementIds.LISTED_USER)
                    .remove();

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

                users.fadeIn(150);
            });
        });
}