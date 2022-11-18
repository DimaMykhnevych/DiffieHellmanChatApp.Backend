using ChatApp.Factories.AuthTokenFactory;
using ChatApp.Models;
using ChatApp.Repositories;
using System.Security.Claims;

namespace ChatApp.Services.AuthorizationService
{
    public class AppUserAuthorizationService : BaseAuthorizationService
    {
        private readonly UserRepository _userRepository;
        public AppUserAuthorizationService(IAuthTokenFactory tokenFactory, UserRepository userRepository) : base(tokenFactory)
        {
            _userRepository = userRepository;
        }

        public override async Task<IEnumerable<Claim>> GetUserClaimsAsync(string username)
        {
            User user = _userRepository.GetUserByUserName(username);
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, username),
                new Claim("Id", user.Id.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            };
            return await Task.FromResult(claims);
        }

        public async override Task<User> GetUserInfoAsync(string username)
        {
            if (username == null) return null;
            User user = _userRepository.GetUserByUserName(username);

            return await Task.FromResult(user);
        }
    }
}
