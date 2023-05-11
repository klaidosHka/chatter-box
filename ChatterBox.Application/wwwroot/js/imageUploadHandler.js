
function validateForm() {
    var allowedTypes = ['image/png', 'image/jpeg', 'image/svg'];
    var fileInput = document.querySelector('input[name="photo"]');
    var file = fileInput.files[0];

    if (!file || allowedTypes.indexOf(file.type) === -1)
    {
        alert('Prašome pasirinkti tinkamo formato failą (.jpg, .png, .svg.)');
        return false;
    }
    return true;
}
