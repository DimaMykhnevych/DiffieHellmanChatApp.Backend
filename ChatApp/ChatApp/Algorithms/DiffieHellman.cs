using ChatApp.DTOs;
using ChatApp.Models;
using ChatApp.Repositories;

namespace ChatApp.Algorithms
{
    public class DiffieHellman
    {
        private readonly ChatParticipantsRepository _chatParticipantsRepository;
        private Guid? firstSenderId = null;
        private static int rotationAmount;

        public DiffieHellman(ChatParticipantsRepository chatParticipantsRepository)
        {
            _chatParticipantsRepository = chatParticipantsRepository;
        }

        public (string, KeyExchangeResponseDto) GetNextReceiver(KeyExchangeDto keyExhangeDto)
        {
            KeyExchangeResponseDto result = new() { PublicKey = keyExhangeDto.PublicKey, IsLastExchange = false };
            int chatParticipantIndex = _chatParticipantsRepository.GetParticipantIndex(keyExhangeDto.SenderId);
            if (chatParticipantIndex + 1 >= _chatParticipantsRepository.ChatParticipantsAmount || 
                rotationAmount == _chatParticipantsRepository.ChatParticipantsAmount)
            {
                rotationAmount = 0;
                firstSenderId = null;
                return (null, null);
            }
            else
            {
                ChatParticipant receiver = _chatParticipantsRepository.GetChatParticipantByIndex(chatParticipantIndex + 1);
                if (chatParticipantIndex + 2 == _chatParticipantsRepository.ChatParticipantsAmount)
                {
                    result.IsLastExchange = true;
                    rotationAmount++;
                    _chatParticipantsRepository.RotateChatParticipantsList();
                }
                return (receiver.ConnectionId, result);
            }
        }

        public void SetFirstSenderId(Guid id)
        {
            if(firstSenderId == null)
            {
                firstSenderId = id;
                _chatParticipantsRepository.SetChatParticipantToFirstPlace(firstSenderId.Value);
            }
        }
    }
}
