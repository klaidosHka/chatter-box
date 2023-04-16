using ChatterBox.Interfaces.Entities;

namespace ChatterBox.Interfaces.Services
{
    public interface IChatMessageService
    {
        IEnumerable<ChatMessage> GetMessages();

        IEnumerable<ChatMessage> GetMessagesAsNoTracking();

        Task ImportAsync(ChatMessage message);

        Task ImportAsync(IEnumerable<ChatMessage> messages);
    }
}
