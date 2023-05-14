using ChatterBox.Interfaces.Entities;
using ChatterBox.Interfaces.Repositories;
using ChatterBox.Interfaces.Services;
using System.Data.Entity;
using ChatterBox.Interfaces.Dto;

namespace ChatterBox.Services.Services
{
    public class ChatGroupMessageService : IChatGroupMessageService
    {
        private readonly IHelperService _helperService;
        private readonly IChatGroupMessageRepository _messageRepository;

        public ChatGroupMessageService(IHelperService helperService,
            IChatGroupMessageRepository chatGroupMessageRepository)
        {
            _helperService = helperService;
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

        public IEnumerable<GroupMessageMapped> GetMapped(string groupId)
        {
            var signalrId = _helperService.ResolveGroupChatId(groupId);

            return GetAsNoTracking()
                .Where(m => m.GroupId == groupId)
                .Select(m => new GroupMessageMapped
                {
                    DateSent = m.DateSent,
                    GroupId = m.GroupId,
                    ImageLink = m.ImageLink,
                    SenderId = m.SenderId,
                    SenderUserName = m.Sender.UserName,
                    SignalrId = signalrId,
                    Text = m.Text
                })
                .OrderBy(m => m.DateSent)
                .ToList();
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