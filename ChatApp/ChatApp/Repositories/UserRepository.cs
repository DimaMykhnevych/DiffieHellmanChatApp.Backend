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
            if(_users.Count(u => u.Username == user.Username) > 0)
            {
                throw new Exception("Username already taken");
            }
            _users.Add(user);
        }

        public void DeleteUser(string username)
        {
            var user = _users.FirstOrDefault(u => u.Username == username);
            if(user != null)
            {
                _users.Remove(user);
            }
        }
    }
}
