const searchInput = document.getElementById('user-search-input');
const userList = document.querySelector('.chat-list');
const userButtons = Array.from(userList.querySelectorAll('.user'));

searchInput.addEventListener(
    'input',
    () => {
        const searchTerm = searchInput.value
            .trim()
            .toLowerCase();

        userButtons.forEach(button => {
            const username = button.dataset.username.toLowerCase();

            if (username.includes(searchTerm)) {
                button.classList.remove('d-none');
            } else {
                button.classList.add('d-none');
            }
        });
    }
);