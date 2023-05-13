using ChatterBox.Interfaces.Dto;
using ChatterBox.Interfaces.Entities;

namespace ChatterBox.Interfaces.Services
{
    public interface IChatGroupMessageService
    {
        IEnumerable<ChatGroupMessage> Get();

        IEnumerable<ChatGroupMessage> GetAsNoTracking();

        IEnumerable<GroupMessage> GetMapped(string groupId);

        Task ImportAsync(ChatGroupMessage message);

        Task ImportAsync(IEnumerable<ChatGroupMessage> messages);
    }
}
