$(document).ready(() => {
    $('.dropdown-toggle').click(e => {
        e.stopPropagation();

        $('.dropdown-menu').hide();

        $(this)
            .siblings('.dropdown-menu')
            .toggle();
    });
});
