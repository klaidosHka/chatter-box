using ChatterBox.Interfaces.Entities;

namespace ChatterBox.Interfaces.Services
{
    public interface IChatUserService
    {
        IEnumerable<ChatUser> Get();

        IEnumerable<ChatUser> GetAsNoTracking();

        void Import(ChatUser user);

        void Import(IEnumerable<ChatUser> users);
    }
}
