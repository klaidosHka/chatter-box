let defaultTypes = 'image/jpeg, image/png, image/gif, image/bmp, image/x-icon, image/webp, image/svg+xml';
let image;

$(document).ready(() => {
    $(chatElementIds.INPUT_FILE).click(e => {
        openImageFileInput();
    });

    function openImageFileInput() {
        let fileInput = $('<input type="file" accept=' + defaultTypes + '>');
        let dialog = $(chatElementIds.WRONG_INPUT_FILE_DIALOG);

        dialog.click(() => {
            dialog[0].close();
        });

        fileInput.change(() => {
            let file = fileInput[0].files[0];
            let types = defaultTypes.split(', ');

            if (!file || !types.includes(file.type)) {
                dialog[0].showModal();

                return;
            }

            $(chatElementIds.INPUT_FILE).addClass("green");

            image = file;
        });

        fileInput.click();
    }

    $(document).on("click", ".message-img", e => {
        let imageLink = $(e.currentTarget).attr("src");

        if (imageLink) {
            $.magnificPopup.open({
                items: {
                    src: imageLink
                },
                image: {
                    markup: '<div class="mfp-figure">' +
                        '<div class="mfp-close"></div>' +
                        '<div class="mfp-img"></div>' +
                        '<div class="mfp-bottom-bar">' +
                        '<div class="mfp-title"></div>' +
                        '<div class="mfp-counter"></div>' +
                        '</div>' +
                        '</div>',
                    cursor: 'mfp-zoom-out-cur',
                },
                type: "image",
                mainClass: 'mfp-with-zoom',
                closeOnContentClick: true
            });
        }
    });
});

