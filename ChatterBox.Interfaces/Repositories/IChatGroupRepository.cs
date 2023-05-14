using ChatterBox.Interfaces.Entities;

namespace ChatterBox.Interfaces.Repositories
{
    public interface IChatGroupRepository
    {
        Task CommitAsync();

        IQueryable<ChatGroup> Get();

        Task ImportAsync(ChatGroup group);

        Task ImportAsync(IEnumerable<ChatGroup> groups);

        Task RemoveAsync(ChatGroup group);
    }
}
