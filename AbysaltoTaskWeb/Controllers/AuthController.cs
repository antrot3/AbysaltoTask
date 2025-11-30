using AplicationLayer.DTOs;
using AplicationLayer.Interfaces;
using DomainLayer.Entites;
using Microsoft.AspNetCore.Mvc;

namespace AbysaltoTaskWeb.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _users;
        private readonly ITokenService _tokens;
        private readonly ICartRepository _cartRepository;

        public AuthController(IUserRepository users, ITokenService tokens, ICartRepository cartRepository)
        {
            _users = users;
            _tokens = tokens;
            _cartRepository = cartRepository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var exists = await _users.GetByEmailAsync(dto.Email);
            if (exists != null)
                return BadRequest("Email already in use");

            var hash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var user = new User
            {
                Email = dto.Email,
                FullName = dto.FullName,
                PasswordHash = hash
            };

            await _users.CreateAsync(user);

            return Ok(new { token = _tokens.CreateToken(user) });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await _users.GetByEmailAsync(dto.Email);

            if (user == null) return Unauthorized("Invalid email");

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return Unauthorized("Invalid password");

            await _cartRepository.EnsureCartForUserAsync(user.Id);

            return Ok(new { token = _tokens.CreateToken(user) });
        }
    }
}
