using ChatterBox.Interfaces.Entities;

namespace ChatterBox.Interfaces.Repositories
{
    public interface IChatMessageRepository
    {
        IQueryable<ChatMessage> Get();

        void Import(ChatMessage message);

        void Import(IEnumerable<ChatMessage> messages);
    }
}
