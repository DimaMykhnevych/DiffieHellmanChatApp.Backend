using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ChatApp.Factories.AuthTokenFactory
{
    public interface IAuthTokenFactory
    {
        JwtSecurityToken CreateToken(string username, IEnumerable<Claim> businessClaims);
        JwtSecurityToken CreateToken(string username, SymmetricSecurityKey secret, String issuer, String audience, IEnumerable<Claim> businessClaims);
    }
}
