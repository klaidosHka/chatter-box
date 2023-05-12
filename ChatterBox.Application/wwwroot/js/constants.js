const chatElementIds = {
    BUTTON_SEND: "#context-button-send",
    CONTEXT_DATA: "#context-data",
    INPUT_EMOTE: "#context-input-emote",
    INPUT_FILE: "#context-input-file",
    INPUT_TEXT: "#context-input-text",
    LISTED_USER: ".context-users-list-user",
    MESSAGES: "#context-messages",
    MESSAGES_PARENT: "#context-messages-parent",
    OWN_AVATAR: "#context-own-avatar",
    TARGET_NAME: "#context-target-name"
}

const chatTypeIds = {
    GROUP: 1,
    DIRECT: 2
};

const contextAttributeIds = {
    CHAT_TYPE: "data-chat-type",
    SIGNALR_GROUP_ID: "data-signalr-group-id",
    USER_ID: "data-user-id",
    USER_TARGET_ID: "data-target-id",
    USER_TARGET_NAME: "data-target-username",
    USER_NAME: "data-user-username"
};

const signalRPrefix = {
    GROUP: "grp__",
    DIRECT: "drct__"
};