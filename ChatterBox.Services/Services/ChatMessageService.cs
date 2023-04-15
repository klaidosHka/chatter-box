using ChatterBox.Interfaces.Entities;
using ChatterBox.Interfaces.Repositories;
using ChatterBox.Interfaces.Services;
using Microsoft.EntityFrameworkCore;

namespace ChatterBox.Services.Services
{
    public class ChatMessageService : IChatMessageService
    {
        private readonly IChatMessageRepository _messageRepository;

        public ChatMessageService(IChatMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public IEnumerable<ChatMessage> GetMessages()
        {
            return _messageRepository.Get();
        }

        public IEnumerable<ChatMessage> GetMessagesAsNoTracking()
        {
            return _messageRepository
                .Get()
                .AsNoTracking();
        }

        public void Import(ChatMessage message)
        {
            _messageRepository.Import(message);
        }

        public void Import(IEnumerable<ChatMessage> messages)
        {
            _messageRepository.Import(messages);
        }
    }
}
