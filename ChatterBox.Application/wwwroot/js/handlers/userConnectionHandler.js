let directChatsByUserId = [];
let groupChatsByGroupId = [];

$(document).ready(() => {
    let connection = getConnection();

    connection.on('OnConnected', userId => handleConnectionStatusChange(userId, true));
    connection.on('OnDisconnected', userId => handleConnectionStatusChange(userId, false));
});

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

function handleOwnAvatar(userId, isOnline) {
    let avatarImage = $(chatElementIds.OWN_AVATAR);

    avatarImage.removeClass("avatar-online avatar-offline");

    avatarImage.addClass(isOnline ? "avatar-online" : "avatar-offline");
}

function handleUserAddToDirectChat(userId, targetId) {
    if (targetId === userId || targetId === getContextValues().userId) {
        return;
    }
    
    getConnection()
        .invoke("AddUserToDirectChat", userId, targetId)
        .then(v => {
            directChatsByUserId[targetId] = v;
        })
        .catch(e => {
            console.error(e.toString());
        });
}