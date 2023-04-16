using ChatterBox.Interfaces.Entities;

namespace ChatterBox.Interfaces.Services
{
    public interface IChatGroupService
    {
        IEnumerable<ChatGroup> GetGroups();

        IEnumerable<ChatGroup> GetGroupsAsNoTracking();

        Task ImportAsync(ChatGroup group);

        Task ImportAsync(IEnumerable<ChatGroup> groups);
    }
}
