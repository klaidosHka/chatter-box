using ChatterBox.Interfaces.Entities;

namespace ChatterBox.Interfaces.Services
{
    public interface IChatGroupService
    {
        IEnumerable<ChatGroup> GetGroups();

        IEnumerable<ChatGroup> GetGroupsAsNoTracking();

        void Import(ChatGroup group);

        void Import(IEnumerable<ChatGroup> groups);
    }
}
