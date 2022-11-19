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

        public int ChatParticipantsAmount => _chatParticipants.Count;

        public IEnumerable<ChatParticipant> GetChatParticipants()
        {
            return _chatParticipants;
        }

        public ChatParticipant GetChatParticipantByIndex(int index)
        {
            if (index >= _chatParticipants.Count || index < 0)
            {
                return null;
            }

            return _chatParticipants[index];
        }

        public void RotateChatParticipantsList()
        {
            var last = _chatParticipants[^1];
            _chatParticipants.RemoveAt(_chatParticipants.Count - 1);
            _chatParticipants.Insert(0, last);
        }

        public void SetChatParticipantToFirstPlace(Guid id)
        {
            var requiredParticipant = _chatParticipants.FirstOrDefault(c => c.User.Id == id);
            if (requiredParticipant != null && _chatParticipants[0].User.Id != id)
            {
                _chatParticipants.Remove(requiredParticipant);
                _chatParticipants.Insert(0, requiredParticipant);
            }
        }

        public int GetParticipantIndex(Guid id)
        {
            var requiredParticipant = _chatParticipants.FirstOrDefault(c => c.User.Id == id);
            return _chatParticipants.IndexOf(requiredParticipant);
        }

        public void AddChatParticipant(string username, string connectinoId)
        {
            User user = _userRepository.GetUserByUserName(username);

            if (_chatParticipants.Count(c => c.User.Username == username) != 0)
            {
                _chatParticipants.Single(c => c.User.Username == username).ConnectionId = connectinoId;
            }
            else
            {
                _chatParticipants.Add(new ChatParticipant { User = user, ConnectionId = connectinoId });
            }
        }

        public void RemoveChatParticipant(string connectinoId)
        {
            var participantToRemove = _chatParticipants.FirstOrDefault(p => p.ConnectionId == connectinoId);

            if (participantToRemove != null)
            {
                _chatParticipants.Remove(participantToRemove);
            }
        }
    }
}
