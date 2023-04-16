using ChatterBox.Interfaces.Entities;
using Microsoft.AspNetCore.SignalR;

namespace ChatterBox.Context
{
    public class ChatHub : Hub
    {
        public async Task SendDirectMessage(ChatMessage message)
        {
            await Clients
                .Group("")
                .SendAsync("ReceiveDirectMessage", message);
        }

        public async Task SendGroupMessage(ChatGroupMessage message)
        {
            await Clients
                .Group("")
                .SendAsync("ReceiveGroupMessage", message);
        }
    }
}
