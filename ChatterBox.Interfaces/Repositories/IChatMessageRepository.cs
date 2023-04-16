using ChatterBox.Interfaces.Entities;

namespace ChatterBox.Interfaces.Repositories
{
    public interface IChatMessageRepository
    {
        IQueryable<ChatMessage> Get();

        Task ImportAsync(ChatMessage message);

        Task ImportAsync(IEnumerable<ChatMessage> messages);
    }
}
