using ChatterBox.Interfaces.Entities;
using ChatterBox.Interfaces.Repositories;
using ChatterBox.Interfaces.Services;
using Microsoft.EntityFrameworkCore;

namespace ChatterBox.Services.Services
{
    public class GroupService : IGroupService
    {

        private readonly IGroupRepository _groupRepository;

        public GroupService(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }

        public IEnumerable<Group> GetGroups()
        {
            return _groupRepository
                .Get()
                .AsEnumerable();
        }

        public IEnumerable<Group> GetGroupsAsNoTracking()
        {
            return _groupRepository
                .Get()
                .AsNoTracking()
                .AsEnumerable();
        }

        public void Import(Group group)
        {
            _groupRepository.Import(group);
        }

        public void Import(IEnumerable<Group> groups)
        {
            _groupRepository.Import(groups);
        }
    }
}
