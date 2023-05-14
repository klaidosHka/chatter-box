using ChatterBox.Interfaces.Dto;
using ChatterBox.Interfaces.Entities;

namespace ChatterBox.Interfaces.Services
{
    public interface IChatGroupService
    {
        public Task<CreateGroupResponse> CreateAsync(CreateGroupRequest request);
        
        IEnumerable<ChatGroup> Get();

        IEnumerable<ChatGroup> GetAsNoTracking();

        IEnumerable<GroupMapped> GetMapped(string userId);

        IEnumerable<ChatGroup> GetUserGroups(string userId);

        Task ImportAsync(ChatGroup group);

        Task ImportAsync(IEnumerable<ChatGroup> groups);
    }
}
