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
});

