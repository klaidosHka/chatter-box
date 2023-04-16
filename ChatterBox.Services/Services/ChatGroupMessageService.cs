using ChatterBox.Interfaces.Entities;
using ChatterBox.Interfaces.Repositories;
using ChatterBox.Interfaces.Services;
using System.Data.Entity;

namespace ChatterBox.Services.Services
{
    public class ChatGroupMessageService : IChatGroupMessageService
    {
        private readonly IChatGroupMessageRepository _messageRepository;

        public ChatGroupMessageService(IChatGroupMessageRepository chatGroupMessageRepository)
        {
            _messageRepository = chatGroupMessageRepository;
        }

        public IEnumerable<ChatGroupMessage> Get()
        {
            return _messageRepository.Get();
        }

        public IEnumerable<ChatGroupMessage> GetAsNoTracking()
        {
            return _messageRepository
                .Get()
                .AsNoTracking();
        }

        public async Task ImportAsync(ChatGroupMessage message)
        {
            await _messageRepository.ImportAsync(message);
        }

        public async Task ImportAsync(IEnumerable<ChatGroupMessage> messages)
        {
            await _messageRepository.ImportAsync(messages);
        }
    }
}
