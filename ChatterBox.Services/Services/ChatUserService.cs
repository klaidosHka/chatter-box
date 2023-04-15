using ChatterBox.Interfaces.Entities;
using ChatterBox.Interfaces.Repositories;
using ChatterBox.Interfaces.Services;
using Microsoft.EntityFrameworkCore;

namespace ChatterBox.Services.Services
{
    public class ChatUserService : IChatUserService
    {
        private readonly IChatUserRepository _userRepository;

        public ChatUserService(IChatUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IEnumerable<ChatUser> Get()
        {
            return _userRepository.Get();
        }

        public IEnumerable<ChatUser> GetAsNoTracking()
        {
            return _userRepository
                .Get()
                .AsNoTracking();
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
