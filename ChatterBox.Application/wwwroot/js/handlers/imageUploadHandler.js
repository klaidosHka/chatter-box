function isFileInputValid() {
    let allowedTypes = ['image/png', 'image/jpeg', 'image/svg'];
    let fileInput = document.querySelector('input[name="photo"]');
    let file = fileInput.files[0];

    if (!file || allowedTypes.indexOf(file.type) === -1) {
        alert('Please choose an appropriate image file (.jpg, .png, .svg.)');

        return false;
    }

    return true;
}