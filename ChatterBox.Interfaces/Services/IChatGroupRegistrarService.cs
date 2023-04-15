using ChatterBox.Interfaces.Entities;

namespace ChatterBox.Interfaces.Services
{
    public interface IChatGroupRegistrarService
    {
        IEnumerable<ChatGroupRegistrar> Get();

        IEnumerable<ChatGroupRegistrar> GetAsNoTracking();

        void Import(ChatGroupRegistrar group);

        void Import(IEnumerable<ChatGroupRegistrar> groups);
    }
}
