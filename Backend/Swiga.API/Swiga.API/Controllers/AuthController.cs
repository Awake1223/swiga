using Microsoft.AspNetCore.Mvc;
using Swiga.API.Contracts;
using Swiga.Application.Services;
using Swiga.Application.Services.Security;
using Swiga.Infrastructure.Repositories;

namespace Swiga.API.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenService _jwt;
        
        public AuthController(IUserService userService, IPasswordHasher passwordHasher, IJwtTokenService jwt)
        {
            _userService = userService;
            _passwordHasher = passwordHasher;
            _jwt = jwt;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _userService.GetUserByEmailAsync(request.Email);

            if(user == null)
                return Unauthorized( new { error = "Invalid email or password" });

            var ok = _passwordHasher.VerifyPassword(request.Password, user.Password);

            if (!ok)
                return Unauthorized(new { error = "Ivalid email or password" });

            var (token, expiresAtUtc) = _jwt.Create(user);

            return Ok(new
            {
                userId = user.Id,
                email = user.Email,
                role = user.Role.ToString(),
                accessToken = token,
                expiresAtUtc
            });
        }


    }

}
