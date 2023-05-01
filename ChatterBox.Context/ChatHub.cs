using ChatterBox.Interfaces.Entities;
using Microsoft.AspNetCore.SignalR;

namespace ChatterBox.Context
{
    public class ChatHub : Hub
    {
        public async Task SendDirectMessage(string message)
        {
            await Clients.All.SendAsync("ReceiveDirectMessage", message);
        }

        public async Task SendGroupMessage(ChatGroupMessage message)
        {
            await Clients.All.SendAsync("ReceiveGroupMessage", message);
        }
    }
}
