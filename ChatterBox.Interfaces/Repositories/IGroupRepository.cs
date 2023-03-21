using ChatterBox.Interfaces.Entities;

namespace ChatterBox.Interfaces.Repositories
{
    public interface IGroupRepository
    {
        IQueryable<Group> Get();

        void Import(Group group);

        void Import(IEnumerable<Group> groups);
    }
}
