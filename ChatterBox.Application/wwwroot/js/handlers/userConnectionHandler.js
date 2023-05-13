let directChatsByUserId = [];
let groupChatsByGroupId = [];

$(document).ready(() => {
    let connection = getConnection();

    connection.on('OnConnected', userId => handleConnectionStatusChange(userId, true));
    connection.on('OnDisconnected', userId => handleConnectionStatusChange(userId, false));
});

function getUser(id) {
    return $(chatElementIds.LISTED_USER + "[" + targetAttributeIds.ID + "='" + id + "']");
}

function handleConnectionStatusChange(userId, isOnline) {
    let context = getContextValues();

    if (context.userId === userId) {
        handleOwnAvatar(userId, isOnline);
    }

    handleListedUser(userId, isOnline);
    
    restructureUsersList();
}

function handleListedUser(userId, isOnline) {
    let user = getUser(userId);
    
    user.attr(targetAttributeIds.ONLINE, isOnline);
    
    let image = user.find("img");

    image.removeClass("avatar-online avatar-offline");

    image.addClass(isOnline ? "avatar-online" : "avatar-offline");
}

function handleOwnAvatar(userId, isOnline) {
    let image = $(chatElementIds.OWN_AVATAR);

    image.removeClass("avatar-online avatar-offline");

    image.addClass(isOnline ? "avatar-online" : "avatar-offline");
}

function handleUserAddToDirectChat(userId, targetId) {
    if (targetId === userId || targetId === getContextValues().userId) {
        return;
    }

    getConnection()
        .invoke("AddUserToDirectChat", userId, targetId)
        .then(v => {
            directChatsByUserId[targetId] = v;
        });
}

function handleUserAddToGroupChat(userId, targetId) {
    if (targetId === userId || targetId === getContextValues().userId) {
        return;
    }

    getConnection()
        .invoke("AddUserToGroupChat", userId, targetId)
        .then(v => {
            groupChatsByGroupId[targetId] = v;
        });
}