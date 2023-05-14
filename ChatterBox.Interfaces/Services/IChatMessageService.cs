using ChatterBox.Interfaces.Dto;
using ChatterBox.Interfaces.Entities;

namespace ChatterBox.Interfaces.Services
{
    public interface IChatMessageService
    {
        IEnumerable<ChatMessage> Get();

        IEnumerable<ChatMessage> GetAsNoTracking();

        IEnumerable<MessageMapped> GetMapped(string userIdFirst, string userIdSecond);

        Task ImportAsync(ChatMessage message);

        Task ImportAsync(IEnumerable<ChatMessage> messages);
    }
}
