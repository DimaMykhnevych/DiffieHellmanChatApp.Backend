using ChatApp.DTOs;
using ChatApp.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly ChatParticipantsRepository _chatParticipantsRepository;
        private readonly ILogger<ChatHub> _logger;

        public ChatHub(ChatParticipantsRepository chatParticipantsRepository, ILogger<ChatHub> logger)
        {
            _chatParticipantsRepository = chatParticipantsRepository;
            _logger = logger;
        }

        public Task SendMessage(MessageDto message)
        {
            return Clients.Others.SendAsync("ReceiveMessage", message);
        }

        public override async Task OnConnectedAsync()
        {
            _logger.LogInformation($"{Context.UserIdentifier} joined the chat. Connection id: {Context.ConnectionId}");
            _chatParticipantsRepository.AddChatParticipant(Context.UserIdentifier, Context.ConnectionId);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception ex)
        {
            _logger.LogInformation($"{Context.UserIdentifier} left the chat. Connection id: {Context.ConnectionId}");
            _chatParticipantsRepository.RemoveChatParticipant(Context.ConnectionId);
            await base.OnDisconnectedAsync(ex);
        }
    }
}
