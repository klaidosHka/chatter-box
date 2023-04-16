using ChatterBox.Interfaces.Entities;

namespace ChatterBox.Interfaces.Repositories
{
    public interface IChatGroupRepository
    {
        IQueryable<ChatGroup> Get();

        Task ImportAsync(ChatGroup group);

        Task ImportAsync(IEnumerable<ChatGroup> groups);
    }
}
