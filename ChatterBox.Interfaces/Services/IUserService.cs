using ChatterBox.Interfaces.Entities;

namespace ChatterBox.Interfaces
{
    public interface IUserService
    {
        IEnumerable<ChatUser> GetUsers();

        IEnumerable<ChatUser> GetUsersAsNoTracking();

        void Import(ChatUser user);

        void Import(IEnumerable<ChatUser> users);
    }
}
