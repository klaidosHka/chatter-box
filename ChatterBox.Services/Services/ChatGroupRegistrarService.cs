using ChatterBox.Interfaces.Entities;
using ChatterBox.Interfaces.Repositories;
using ChatterBox.Interfaces.Services;
using Microsoft.EntityFrameworkCore;

namespace ChatterBox.Services.Services
{
    public class ChatGroupRegistrarService : IChatGroupRegistrarService
    {
        private readonly IChatGroupRegistrarRepository _registrarRepository;

        public ChatGroupRegistrarService(IChatGroupRegistrarRepository registrarRepository)
        {
            _registrarRepository = registrarRepository;
        }

        public IEnumerable<ChatGroupRegistrar> Get()
        {
            return _registrarRepository.Get();
        }

        public IEnumerable<ChatGroupRegistrar> GetAsNoTracking()
        {
            return _registrarRepository
                .Get()
                .AsNoTracking()
                .Include(r => r.User);
        }

        public async Task ImportAsync(ChatGroupRegistrar registrar)
        {
            await _registrarRepository.ImportAsync(registrar);
        }

        public async Task ImportAsync(IEnumerable<ChatGroupRegistrar> registrars)
        {
            await _registrarRepository.ImportAsync(registrars);
        }

        public async Task<bool> JoinGroupAsync(string userId, string groupId)
        {
            if (GetAsNoTracking().Any(r => r.GroupId == groupId && r.UserId == userId))
            {
                return false;
            }

            await ImportAsync(new ChatGroupRegistrar
            {
                Id = Guid.NewGuid().ToString(),
                GroupId = groupId,
                UserId = userId
            });

            return true;
        }

        public async Task<bool> LeaveGroupAsync(string userId, string groupId)
        {
            var group = GetAsNoTracking().FirstOrDefault(r => r.GroupId == groupId && r.UserId == userId);
            
            if (group is null)
            {
                return false;
            }

            await _registrarRepository.RemoveAsync(group);

            return true;
        }
    }
}