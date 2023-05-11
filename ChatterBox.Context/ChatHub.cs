using ChatterBox.Interfaces.Dto;
using ChatterBox.Interfaces.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

namespace ChatterBox.Context
{
    public class ChatHub : Hub
    {
        public static readonly IDictionary<string, string> OnlineUsers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        
        private readonly UserManager<ChatUser> _userManager;

        public ChatHub(UserManager<ChatUser> userManager)
        {
            _userManager = userManager;
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
            await Clients.All.SendAsync(
                "ReceiveMessage",
                new SendMessageResponseRequest
                {
                    DateSent = request.DateSent,
                    Id = request.SenderId,
                    UserName = _userManager.Users
                        .AsEnumerable()
                        .FirstOrDefault(u => u.Id.ToString() == request.SenderId, new ChatUser()).UserName,
                    Text = request.Text
                }
            );
        }

        public async Task SendGroupMessage(SendMessageRequest request)
        {
            await Clients.All.SendAsync(
                "ReceiveMessage",
                new SendMessageResponseRequest
                {
                    DateSent = request.DateSent,
                    Id = request.SenderId,
                    UserName = _userManager.Users
                        .FirstOrDefault(u => u.Id.ToString() == request.SenderId, new ChatUser()).UserName,
                    Text = request.Text
                }
            );
        }
    }
}