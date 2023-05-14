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
                console.log(response);

                $(chatElementIds.GROUPS_DIALOG)[0].close();
            })
            .fail(xhr => {
                console.log(!xhr.responseJSON);
                console.log(!xhr.responseJSON.messageError);

                if (!xhr.responseJSON || !xhr.responseJSON.messageError) {
                    return;
                }

                $(groupElementIds.CREATION_INFO).text(xhr.responseJSON.messageError);
            });
    });
});