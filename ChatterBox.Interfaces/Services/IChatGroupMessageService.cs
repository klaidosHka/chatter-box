using ChatterBox.Interfaces.Entities;

namespace ChatterBox.Interfaces.Services
{
    public interface IChatGroupMessageService
    {
        IEnumerable<ChatGroupMessage> Get();

        IEnumerable<ChatGroupMessage> GetAsNoTracking();

        void Import(ChatGroupMessage message);

        void Import(IEnumerable<ChatGroupMessage> messages);
    }
}
