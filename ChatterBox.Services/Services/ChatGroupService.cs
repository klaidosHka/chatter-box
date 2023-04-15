using ChatterBox.Interfaces.Entities;
using ChatterBox.Interfaces.Repositories;
using ChatterBox.Interfaces.Services;
using Microsoft.EntityFrameworkCore;

namespace ChatterBox.Services.Services
{
    public class ChatGroupService : IChatGroupService
    {

        private readonly IChatGroupRepository _groupRepository;

        public ChatGroupService(IChatGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }

        public IEnumerable<ChatGroup> GetGroups()
        {
            return _groupRepository.Get();
        }

        public IEnumerable<ChatGroup> GetGroupsAsNoTracking()
        {
            return _groupRepository
                .Get()
                .AsNoTracking();
        }

        public void Import(ChatGroup group)
        {
            _groupRepository.Import(group);
        }

        public void Import(IEnumerable<ChatGroup> groups)
        {
            _groupRepository.Import(groups);
        }
    }
}
