const inputSearch = $('#user-search-input');
const users = $(chatElementIds.LISTED_USER);

inputSearch.on('input', () => {
    const searchTerm = inputSearch
        .val()
        .trim()
        .toLowerCase();

    users.each((i, u) => {        
        const username = $(u)
            .data('target-username')
            .toLowerCase();

        if (username.includes(searchTerm)) {
            $(u).removeClass("d-none");
        } else {
            $(u).addClass("d-none");
        }
    });
});