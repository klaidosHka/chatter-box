using ChatterBox.Interfaces.Entities;
using ChatterBox.Interfaces.Repositories;
using ChatterBox.Interfaces.Services;
using Microsoft.EntityFrameworkCore;

namespace ChatterBox.Services.Services
{
    public class ChatGroupService : IChatGroupService
    {
        private readonly IChatGroupRegistrarService _registrarService;
        private readonly IChatGroupRepository _repository;

        public ChatGroupService(IChatGroupRegistrarService registrarService, IChatGroupRepository repository)
        {
            _registrarService = registrarService;
            _repository = repository;
        }

        public IEnumerable<ChatGroup> GetGroups()
        {
            return _repository.Get();
        }

        public IEnumerable<ChatGroup> GetGroupsAsNoTracking()
        {
            return _repository
                .Get()
                .AsNoTracking();
        }

        public IEnumerable<ChatGroup> GetUserGroups(string userId)
        {
            return _registrarService
                .GetAsNoTracking()
                .Select(r => new
                {
                    r.UserId,
                    r.GroupId
                })
                .Where(r => r.UserId == userId)
                .Join(
                    GetGroupsAsNoTracking(),
                    r => r.GroupId,
                    g => g.Id,
                    (r, g) => g
                )
                .ToList();
        }

        public async Task ImportAsync(ChatGroup group)
        {
            await _repository.ImportAsync(group);
        }

        public async Task ImportAsync(IEnumerable<ChatGroup> groups)
        {
            await _repository.ImportAsync(groups);
        }
    }
}