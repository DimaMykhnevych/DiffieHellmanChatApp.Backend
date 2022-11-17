using ChatApp.DTOs;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.Hubs
{
    public class ChatHub : Hub
    {
        public Task SendMessage(MessageDto message)
        {
            return Clients.Others.SendAsync("ReceiveMessage", message);
        }
    }
}
