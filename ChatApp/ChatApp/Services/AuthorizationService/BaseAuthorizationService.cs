using ChatApp.Factories.AuthTokenFactory;
using ChatApp.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ChatApp.Services.AuthorizationService
{
    public abstract class BaseAuthorizationService
    {
        private readonly IAuthTokenFactory _tokenFactory;
        public BaseAuthorizationService(IAuthTokenFactory tokenFactory)
        {
            _tokenFactory = tokenFactory;
        }
        public async Task<JWTTokenStatusResult> GenerateTokenAsync(string username)
        {
            IEnumerable<Claim> claims = await GetUserClaimsAsync(username);
            JwtSecurityToken token = _tokenFactory.CreateToken(username, claims);

            return new JWTTokenStatusResult()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                IsAuthorized = true,
                UserInfo = await GetUserInfoAsync(username),
            };
        }

        public abstract Task<IEnumerable<Claim>> GetUserClaimsAsync(string username);
        public abstract Task<User> GetUserInfoAsync(string userName);
    }
}
