using ChatterBox.Interfaces.Entities;

namespace ChatterBox.Interfaces.Services
{
    public interface IGroupService
    {
        IEnumerable<Group> GetGroups();

        IEnumerable<Group> GetGroupsAsNoTracking();

        void Import(Group group);

        void Import(IEnumerable<Group> groups);
    }
}
