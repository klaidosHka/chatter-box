using ChatterBox.Interfaces.Entities;

namespace ChatterBox.Interfaces.Services
{
    public interface IChatGroupMessageService
    {
        IEnumerable<ChatGroupMessage> Get();

        IEnumerable<ChatGroupMessage> GetAsNoTracking();

        Task ImportAsync(ChatGroupMessage message);

        Task ImportAsync(IEnumerable<ChatGroupMessage> messages);
    }
}
