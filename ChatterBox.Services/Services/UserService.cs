using ChatterBox.Interfaces;
using ChatterBox.Interfaces.Entities;
using ChatterBox.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ChatterBox.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IEnumerable<ChatUser> GetUsersAsNoTracking()
        {
            return _userRepository
                .Get()
                .AsNoTracking()
                .AsEnumerable();
        }

        public IEnumerable<ChatUser> GetUsers()
        {
            return _userRepository
                .Get()
                .AsEnumerable();
        }

        public void Import(ChatUser user)
        {
            _userRepository.Import(user);
        }

        public void Import(IEnumerable<ChatUser> users)
        {
            _userRepository.Import(users);
        }
    }
}
