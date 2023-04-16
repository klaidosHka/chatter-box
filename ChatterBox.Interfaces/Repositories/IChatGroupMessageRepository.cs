using ChatterBox.Interfaces.Entities;

namespace ChatterBox.Interfaces.Repositories
{
    public interface IChatGroupMessageRepository
    {
        IQueryable<ChatGroupMessage> Get();

        Task ImportAsync(ChatGroupMessage message);

        Task ImportAsync(IEnumerable<ChatGroupMessage> messages);
    }
}
