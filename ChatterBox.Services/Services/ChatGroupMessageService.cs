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

        public void Import(ChatGroupMessage message)
        {
            _messageRepository.Import(message);
        }

        public void Import(IEnumerable<ChatGroupMessage> messages)
        {
            _messageRepository.Import(messages);
        }
    }
}
