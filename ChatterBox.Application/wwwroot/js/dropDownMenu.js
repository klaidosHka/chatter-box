    $(document).ready(function() {
        $('.dropdown-toggle').click(function (e) {
            e.stopPropagation();
            $('.dropdown-menu').hide();
            $(this).siblings('.dropdown-menu').toggle();
        });

    $(document).click(function() {
        $('.dropdown-menu').hide();
        });
    });
