using ChatterBox.Interfaces.Entities;

namespace ChatterBox.Interfaces.Repositories
{
    public interface IChatGroupMessageRepository
    {
        IQueryable<ChatGroupMessage> Get();

        void Import(ChatGroupMessage message);

        void Import(IEnumerable<ChatGroupMessage> messages);
    }
}
