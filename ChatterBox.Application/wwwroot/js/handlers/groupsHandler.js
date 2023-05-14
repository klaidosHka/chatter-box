$(document).ready(() => {
    $(chatElementIds.LISTED_GROUP).click(e => {
        let target = $(e.currentTarget);

        handleContextSwitch(target, chatTypeIds.GROUP);

        updateUsersForGroup(target.attr(targetAttributes.ID));
    });

    $(chatElementIds.GROUPS_BUTTON_OPEN).click(e => {
        $(groupElementIds.CREATION_INFO).text(null);

        $(chatElementIds.GROUPS_DIALOG)[0].showModal();
    });

    $(chatElementIds.GROUPS_BUTTON_CLOSE).click(e => {
        $(groupElementIds.CREATION_INFO).text(null);

        $(chatElementIds.GROUPS_DIALOG)[0].close();
    });

    $(chatElementIds.GROUP_MORE).click(e => {
        let inputName = $(groupElementIds.INPUT_NAME_CHANGE).val($(chatElementIds.CONTEXT_DATA).attr(contextAttributes.TARGET_NAME));
        let nameChangeSubmit = $(groupElementIds.NAME_CHANGE_SUBMIT);
        let leaveButton = $(groupElementIds.LEAVE);
        let deleteButton = $(groupElementIds.DELETE);
        let context = getContextValues();

        if (context.targetOwnerId !== context.userId) {
            inputName.attr("disabled", true);

            nameChangeSubmit.attr("disabled", true);

            leaveButton.show();
            
            $("#header-of-group-more-leave-delete").text("Leave Group");

            deleteButton.hide();
        } else {
            inputName.attr("disabled", false);

            nameChangeSubmit.attr("disabled", false);

            leaveButton.hide();
            
            $("#header-of-group-more-leave-delete").text("Delete Group");

            deleteButton.show();
        }

        $(groupElementIds.NAME_CHANGE_INFO).text(null);

        $(chatElementIds.GROUP_MORE_DIALOG)[0].showModal();
    });

    $(chatElementIds.GROUP_MORE_DIALOG_CLOSE).click(e => {
        $(chatElementIds.GROUP_MORE_DIALOG)[0].close();
    });

    $(groupElementIds.NAME_CHANGE_SUBMIT).click(e => {
        let context = getContextValues();

        $
            .ajax({
                url: '/Main/Index?handler=RenameGroup',
                method: 'POST',
                data: {
                    groupId: context.targetId,
                    name: $(groupElementIds.INPUT_NAME_CHANGE).val()
                },
                dataType: 'json'
            })
            .done(response => {
                $(chatElementIds.GROUP_MORE_DIALOG)[0].close();

                location.reload();
            })
            .fail(xhr => {
                if (!xhr.responseJSON || !xhr.responseJSON.messageError) {
                    return;
                }

                $(groupElementIds.NAME_CHANGE_INFO).text(xhr.responseJSON.messageError);
            });
    });

    $(groupElementIds.LEAVE).click(e => {
        let context = getContextValues();

        $
            .ajax({
                url: '/Main/Index?handler=LeaveGroup',
                method: 'POST',
                data: {
                    groupId: context.targetId
                },
                dataType: 'json'
            })
            .always(() => {
                $(chatElementIds.GROUP_MORE_DIALOG)[0].close();

                location.reload();
            });
    });

    $(groupElementIds.DELETE).click(e => {
        let context = getContextValues();

        $
            .ajax({
                url: '/Main/Index?handler=DeleteGroup',
                method: 'POST',
                data: {
                    groupId: context.targetId
                },
                dataType: 'json'
            })
            .always(() => {
                $(chatElementIds.GROUP_MORE_DIALOG)[0].close();

                location.reload();
            });
    });

    $(groupElementIds.JOIN).click(e => {
        let target = $(e.currentTarget);
        let groupId = target.attr(groupAttributes.ID);

        $
            .ajax({
                url: '/Main/Index?handler=JoinGroup',
                method: 'POST',
                data: {
                    groupId: groupId
                },
                dataType: 'json'
            })
            .always(() => {
                $(chatElementIds.GROUPS_DIALOG)[0].close();

                location.reload();
            });
    });

    $(groupElementIds.CREATION_SUBMIT).click(e => {
        let inputName = $(groupElementIds.INPUT_NAME);
        let groupName = inputName
            .val()
            .trim();

        inputName.val(null);

        if (groupName === "") {
            return;
        }

        $
            .ajax({
                url: '/Main/Index?handler=CreateGroup',
                method: 'POST',
                data: {
                    groupName: groupName
                },
                dataType: 'json'
            })
            .done(response => {
                $(chatElementIds.GROUPS_DIALOG)[0].close();

                location.reload();
            })
            .fail(xhr => {
                if (!xhr.responseJSON || !xhr.responseJSON.messageError) {
                    return;
                }

                $(groupElementIds.CREATION_INFO).text(xhr.responseJSON.messageError);
            });
    });
});