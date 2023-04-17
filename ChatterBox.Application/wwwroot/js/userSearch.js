function searchUsers(searchValue) {
    var users = document.querySelectorAll('.chat-list');
    var searchRegex = new RegExp(searchValue, 'i');

    users.forEach(function (user) {
        var name = user.querySelector('.name').innerText;
        if (searchRegex.test(name)) {
            user.style.display = 'block';
        } else {
            user.style.display = 'none';
        }
    });
}