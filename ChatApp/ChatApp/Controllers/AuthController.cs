using ChatApp.Models;
using ChatApp.Repositories;
using ChatApp.Services.AuthorizationService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly BaseAuthorizationService _authorizationService;
        private readonly UserRepository _userRepository;
        public AuthController(BaseAuthorizationService authorizationService, UserRepository userRepository)
        {
            _authorizationService = authorizationService;
            _userRepository = userRepository;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("token")]
        public async Task<IActionResult> Login([FromBody] AuthSignInModel model)
        {
            _userRepository.AddUser(new User { Id = Guid.NewGuid(), Username = model.Username });
            JWTTokenStatusResult result = await _authorizationService.GenerateTokenAsync(model.Username);
            return Ok(result);
        }
    }
}
