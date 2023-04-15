using ChatterBox.Interfaces.Entities;

namespace ChatterBox.Interfaces.Repositories
{
    public interface IChatGroupRepository
    {
        IQueryable<ChatGroup> Get();

        void Import(ChatGroup group);

        void Import(IEnumerable<ChatGroup> groups);
    }
}
