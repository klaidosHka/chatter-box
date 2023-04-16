using ChatterBox.Interfaces.Entities;

namespace ChatterBox.Interfaces.Repositories
{
    public interface IChatGroupRegistrarRepository
    {
        IQueryable<ChatGroupRegistrar> Get();

        Task ImportAsync(ChatGroupRegistrar group);

        Task ImportAsync(IEnumerable<ChatGroupRegistrar> groups);
    }
}
