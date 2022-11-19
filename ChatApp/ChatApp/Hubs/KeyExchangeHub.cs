using ChatApp.Algorithms;
using ChatApp.DTOs;
using ChatApp.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.Hubs
{
    [Authorize]
    public class KeyExchangeHub : Hub
    {
        private readonly ILogger<KeyExchangeHub> _logger;
        private readonly DiffieHellman _diffieHellman;
        private readonly ChatParticipantsRepository _chatParticipantsRepository;

        public KeyExchangeHub(ILogger<KeyExchangeHub> logger, DiffieHellman diffieHellman, ChatParticipantsRepository chatParticipantsRepository)
        {
            _logger = logger;
            _diffieHellman = diffieHellman;
            _chatParticipantsRepository = chatParticipantsRepository;
        }

        public async Task SendPublicKey(KeyExchangeDto keyExhangeDto)
        {
            _diffieHellman.SetFirstSenderId(keyExhangeDto.SenderId);
            var nextReceiver = _diffieHellman.GetNextReceiver(keyExhangeDto);

            if (!string.IsNullOrEmpty(nextReceiver.Item1))
            {
                _logger.LogInformation($"Next receiver connection id: {nextReceiver.Item1}");
                await Clients.Client(nextReceiver.Item1).SendAsync("ReceivePublicKey", nextReceiver.Item2);
            }
            else
            {
                _logger.LogInformation("Key exchange finished");
                await Clients.All.SendAsync("KeyExchangeFinished");
            }
        }


        public override async Task OnConnectedAsync()
        {
            _logger.LogInformation($"{Context.UserIdentifier} connected to key-exchange. Connection id: {Context.ConnectionId}");
            _chatParticipantsRepository.AddChatParticipant(Context.UserIdentifier, Context.ConnectionId);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception ex)
        {
            _logger.LogInformation($"{Context.UserIdentifier} disconnected from key-exchange. Connection id: {Context.ConnectionId}");
            _chatParticipantsRepository.RemoveChatParticipant(Context.ConnectionId);
            await base.OnDisconnectedAsync(ex);
        }
    }
}
