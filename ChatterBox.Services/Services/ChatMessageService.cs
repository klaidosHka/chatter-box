using System.Data.Entity;
using ChatterBox.Interfaces.Entities;
using ChatterBox.Interfaces.Repositories;
using ChatterBox.Interfaces.Services;

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

        public async Task ImportAsync(ChatMessage message)
        {
            await _messageRepository.ImportAsync(message);
        }

        public async Task ImportAsync(IEnumerable<ChatMessage> messages)
        {
            await _messageRepository.ImportAsync(messages);
        }
    }
}
