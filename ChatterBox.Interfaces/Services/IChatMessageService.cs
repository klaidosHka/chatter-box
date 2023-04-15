using ChatterBox.Interfaces.Entities;

namespace ChatterBox.Interfaces.Services
{
    public interface IChatMessageService
    {
        IEnumerable<ChatMessage> GetMessages();

        IEnumerable<ChatMessage> GetMessagesAsNoTracking();

        void Import(ChatMessage message);

        void Import(IEnumerable<ChatMessage> messages);
    }
}
