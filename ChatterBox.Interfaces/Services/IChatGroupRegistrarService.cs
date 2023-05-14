using ChatterBox.Interfaces.Entities;

namespace ChatterBox.Interfaces.Services
{
    public interface IChatGroupRegistrarService
    {
        IEnumerable<ChatGroupRegistrar> Get();

        IEnumerable<ChatGroupRegistrar> GetAsNoTracking();

        Task ImportAsync(ChatGroupRegistrar group);

        Task ImportAsync(IEnumerable<ChatGroupRegistrar> groups);

        Task<bool> JoinGroupAsync(string userId, string groupId);
        
        Task<bool> LeaveGroupAsync(string userId, string groupId);
    }
}
