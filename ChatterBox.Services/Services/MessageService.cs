using ChatterBox.Interfaces.Entities;
using ChatterBox.Interfaces.Repositories;
using ChatterBox.Interfaces.Services;
using Microsoft.EntityFrameworkCore;

namespace ChatterBox.Services.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;

        public MessageService(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public IEnumerable<Message> GetMessages()
        {
            return _messageRepository
                .Get()
                .AsEnumerable();
        }

        public IEnumerable<Message> GetMessagesAsNoTracking()
        {
            return _messageRepository
                .Get()
                .AsNoTracking()
                .AsEnumerable();
        }

        public void Import(Message message)
        {
            _messageRepository.Import(message);
        }

        public void Import(IEnumerable<Message> messages)
        {
            _messageRepository.Import(messages);
        }
    }
}
