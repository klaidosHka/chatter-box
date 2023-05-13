using System.Data.Entity;
using ChatterBox.Interfaces.Dto;
using ChatterBox.Interfaces.Entities;
using ChatterBox.Interfaces.Repositories;
using ChatterBox.Interfaces.Services;

namespace ChatterBox.Services.Services
{
    public class ChatMessageService : IChatMessageService
    {
        private readonly IChatMessageRepository _messageRepository;
        private readonly IHelperService _helperService;

        public ChatMessageService(IChatMessageRepository messageRepository, IHelperService helperService)
        {
            _messageRepository = messageRepository;
            _helperService = helperService;
        }

        public IEnumerable<ChatMessage> Get()
        {
            return _messageRepository.Get();
        }

        public IEnumerable<ChatMessage> GetAsNoTracking()
        {
            return _messageRepository
                .Get()
                .AsNoTracking();
        }

        public IEnumerable<Message> GetMapped(string userIdFirst, string userIdSecond)
        {
            var signalrId = _helperService.ResolveDirectChatId(userIdFirst, userIdSecond);

            return GetAsNoTracking()
                .Where(m =>
                    m.SenderId == userIdFirst && m.ReceiverId == userIdSecond ||
                    m.SenderId == userIdSecond && m.ReceiverId == userIdFirst
                )
                .Select(m => new Message
                {
                    DateSent = m.DateSent,
                    ImageLink = m.ImageLink,
                    ReceiverId = m.ReceiverId,
                    SenderId = m.SenderId,
                    SenderUserName = m.Sender.UserName,
                    SignalrId = signalrId,
                    Text = m.Text
                })
                .OrderBy(m => m.DateSent)
                .ToList();
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