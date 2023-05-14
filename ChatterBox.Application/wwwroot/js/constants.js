const chatElementIds = {
    BUTTON_SEND: "#context-button-send",
    CONTEXT_DATA: "#context-data",
    GROUPS_BUTTON_CLOSE: "#context-groups-close",
    GROUPS_BUTTON_OPEN: "#context-groups-button",
    GROUPS_DIALOG: "#groups-dialog",
    HOME_BUTTON: "#context-home-button",
    INPUT_EMOTE: "#context-input-emote",
    INPUT_FILE: "#context-input-file",
    INPUT_TEXT: "#context-input-text",
    LISTED_GROUP: ".context-groups-list-group",
    LISTED_USER: ".context-users-list-user",
    MESSAGES: "#context-messages",
    MESSAGES_PARENT: "#context-messages-parent",
    OWN_AVATAR: "#context-own-avatar",
    SEARCH: "#user-search-input",
    TARGET_NAME: "#context-target-name",
    USERS_LIST: ".chat-list",
    WRONG_INPUT_FILE_DIALOG: "#wrong-input-file-dialog"
}

const chatTypeIds = {
    GROUP: 1,
    DIRECT: 2
};

const contextAttributes = {
    CHAT_TYPE: "data-chat-type",
    TARGET_ID: "data-target-id",
    TARGET_NAME: "data-target-name",
    USER_ID: "data-user-id",
    USER_NAME: "data-user-username"
};

const groupAttributes = {
    ACTION_CREATE_GROUP: "action-create-group",
    ID: "data-group-id"
};

const groupElementIds = {
    INPUT_NAME: "#group-creation-name",
    CREATION_SUBMIT: "#group-creation-submit",
    CREATION_INFO: "#group-creation-submit-info"
};

const targetAttributes = {
    ID: "data-own-id",
    NAME: "data-own-name",
    ONLINE: "data-online",
    UNREAD: "data-unread"
};
