$(document).ready(() => {
    $(document).on('input', chatElementIds.SEARCH, () => {
        const searchTerm = $(chatElementIds.SEARCH)
            .val()
            .trim()
            .toLowerCase();

        const users = $(chatElementIds.LISTED_USER);

        users.each((i, u) => {
            const username = $(u)
                .attr(targetAttributes.NAME)
                .toLowerCase();

            if (username.includes(searchTerm)) {
                $(u).removeClass("d-none");
            } else {
                $(u).addClass("d-none");
            }
        });
    });
});
