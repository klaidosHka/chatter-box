using ChatterBox.Interfaces.Entities;

namespace ChatterBox.Interfaces.Services
{
    public interface IMessageService
    {
        IEnumerable<Message> GetMessages();

        IEnumerable<Message> GetMessagesAsNoTracking();

        void Import(Message message);

        void Import(IEnumerable<Message> messages);
    }
}
