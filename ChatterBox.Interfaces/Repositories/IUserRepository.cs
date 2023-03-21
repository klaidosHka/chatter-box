using ChatterBox.Interfaces.Entities;

namespace ChatterBox.Interfaces.Repositories
{
    public interface IUserRepository
    {
        IQueryable<ChatUser> Get();

        void Import(ChatUser user);

        void Import(IEnumerable<ChatUser> users);
    }
}
