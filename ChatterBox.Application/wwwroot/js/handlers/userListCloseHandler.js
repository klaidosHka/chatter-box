const toggleUsersBtn = document.getElementById('toggle-users');
const usersContainer = document.getElementById('users-container');

toggleUsersBtn.addEventListener(
    'click',
    () => {
        usersContainer.classList.toggle('hidden');
    }
);