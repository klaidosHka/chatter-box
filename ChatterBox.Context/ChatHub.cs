using ChatterBox.Interfaces.Dto;
using ChatterBox.Interfaces.Entities;
using ChatterBox.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

namespace ChatterBox.Context
{
    public class ChatHub : Hub
    {
        private static readonly IDictionary<string, string> OnlineUsers =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        private readonly IHelperService _helperService;
        private readonly IChatMessageService _messageService;
        private readonly UserManager<ChatUser> _userManager;

        public ChatHub(
            IHelperService helperService,
            IChatMessageService messageService,
            UserManager<ChatUser> userManager
        )
        {
            _helperService = helperService;
            _messageService = messageService;
            _userManager = userManager;
        }

        public async Task<string> AddUserToDirectChat(string userId, string targetId)
        {
            var signalrId = _helperService.ResolveDirectChatId(userId, targetId);

            if (OnlineUsers.TryGetValue(userId, out var value))
            {
                await Groups.AddToGroupAsync(value, signalrId);
            }

            if (OnlineUsers.TryGetValue(targetId, out value))
            {
                await Groups.AddToGroupAsync(value, signalrId);
            }

            return signalrId;
        }

        public async Task<string> AddUserToGroupChat(string userId, string groupId)
        {
            var signalrId = _helperService.ResolveGroupChatId(groupId);

            if (OnlineUsers.TryGetValue(userId, out var value))
            {
                await Groups.AddToGroupAsync(value, signalrId);
            }

            return signalrId;
        }

        public static bool IsOnline(string userId)
        {
            if (String.IsNullOrEmpty(userId))
            {
                return false;
            }

            return OnlineUsers.ContainsKey(userId);
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();

            OnlineUsers[Context.UserIdentifier] = Context.ConnectionId;

            await Clients.All.SendAsync("OnConnected", Context.UserIdentifier);
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnDisconnectedAsync(exception);

            OnlineUsers.Remove(Context.UserIdentifier);

            await Clients.All.SendAsync("OnDisconnected", Context.UserIdentifier);
        }

        public async Task SendMessage(SendMessageRequest request)
        {
            await _messageService.ImportAsync(
                new ChatMessage
                {
                    Id = Guid.NewGuid().ToString(),
                    ReceiverId = request.ReceiverId,
                    SenderId = request.SenderId,
                    DateSent = request.DateSent,
                    Text = request.Text
                }
            );

            await Clients.Group(request.SignalrId).SendAsync(
                "ReceiveMessage",
                new SendMessageResponse
                {
                    DateSent = request.DateSent,
                    ReceiverId = request.ReceiverId,
                    SenderId = request.SenderId,
                    SenderUserName = _userManager.Users
                        .AsEnumerable()
                        .FirstOrDefault(u => u.Id.ToString() == request.SenderId, new ChatUser()).UserName,
                    SignalrId = request.SignalrId,
                    Text = request.Text
                }
            );
        }
    }
}