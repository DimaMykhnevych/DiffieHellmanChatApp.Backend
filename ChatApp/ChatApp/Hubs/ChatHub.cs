using ChatApp.DTOs;
using ChatApp.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly ILogger<ChatHub> _logger;
        private readonly UserRepository _userRepository;

        public ChatHub(ILogger<ChatHub> logger, UserRepository userRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
        }

        public Task SendMessage(MessageDto message)
        {
            return Clients.Others.SendAsync("ReceiveMessage", message);
        }

        public override async Task OnConnectedAsync()
        {
            _logger.LogInformation($"{Context.UserIdentifier} joined the chat. Connection id: {Context.ConnectionId}");
            await base.OnConnectedAsync();
            await Clients.All.SendAsync("ReceiveMessage", new MessageDto { Content = $"{Context.UserIdentifier} joined the chat" });
        }

        public override async Task OnDisconnectedAsync(Exception ex)
        {
            _logger.LogInformation($"{Context.UserIdentifier} left the chat. Connection id: {Context.ConnectionId}");
            _userRepository.DeleteUser(Context.UserIdentifier);
            await Clients.All.SendAsync("ReceiveMessage", new MessageDto { Content = $"{Context.UserIdentifier} left the chat" });
            await base.OnDisconnectedAsync(ex);
        }
    }
}
