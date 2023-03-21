using ChatterBox.Interfaces.Entities;

namespace ChatterBox.Interfaces.Repositories
{
    public interface IMessageRepository
    {
        IQueryable<Message> Get();

        void Import(Message message);

        void Import(IEnumerable<Message> messages);
    }
}
