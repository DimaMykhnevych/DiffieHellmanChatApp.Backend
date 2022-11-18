using ChatApp.Models;

namespace ChatApp.Repositories
{
    public class ChatParticipantsRepository
    {
        private readonly List<ChatParticipant> _chatParticipants = new();
        private readonly UserRepository _userRepository;

        public ChatParticipantsRepository(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IEnumerable<ChatParticipant> GetChatParticipants()
        {
            return _chatParticipants;
        }

        public void AddChatParticipant(string username, string connectinoId)
        {
            User user = _userRepository.GetUserByUserName(username);
            _chatParticipants.Add(new ChatParticipant { User = user, ConnectionId = connectinoId });
        }

        public void RemoveChatParticipant(string connectinoId) {
            var participantToRemove = _chatParticipants.FirstOrDefault(p => p.ConnectionId == connectinoId);

            if(participantToRemove != null)
            {
                _chatParticipants.Remove(participantToRemove);
            }
        }
    }
}
