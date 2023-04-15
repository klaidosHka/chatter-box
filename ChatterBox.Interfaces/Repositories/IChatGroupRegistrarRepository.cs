using ChatterBox.Interfaces.Entities;

namespace ChatterBox.Interfaces.Repositories
{
    public interface IChatGroupRegistrarRepository
    {
        IQueryable<ChatGroupRegistrar> Get();

        void Import(ChatGroupRegistrar group);

        void Import(IEnumerable<ChatGroupRegistrar> groups);
    }
}
