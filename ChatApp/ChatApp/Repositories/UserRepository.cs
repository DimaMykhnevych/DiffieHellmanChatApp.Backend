using ChatApp.Models;

namespace ChatApp.Repositories
{
    public class UserRepository
    {
        private readonly List<User> _users = new();

        public IEnumerable<User> GetUsers()
        {
            return _users;
        }

        public User GetUserByUserName(string username)
        {
            return _users.FirstOrDefault(u => u.Username == username);
        }

        public void AddUser(User user)
        {
            _users.Add(user);
        }
    }
}
