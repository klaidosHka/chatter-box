using ChatterBox.Interfaces.Entities;

namespace ChatterBox.Interfaces.Services
{
    public interface IChatGroupRegistrarService
    {
        IEnumerable<ChatGroupRegistrar> Get();

        IEnumerable<ChatGroupRegistrar> GetAsNoTracking();

        Task ImportAsync(ChatGroupRegistrar group);

        Task ImportAsync(IEnumerable<ChatGroupRegistrar> groups);
    }
}
