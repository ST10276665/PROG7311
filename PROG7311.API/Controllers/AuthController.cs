using Microsoft.AspNetCore.Mvc;
using PROG7311.API.Models;
using PROG7311.API.Services;
using System.Threading.Tasks;

namespace PROG7311.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _auth;

        public AuthController(AuthService auth)
        {
            _auth = auth;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest req)
        {
            var (success, message) = await _auth.RegisterAsync(req);
            if (!success) return BadRequest(new { error = message });
            return Ok(new { message });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest req)
        {
            var token = await _auth.LoginAsync(req);
            if (token == null) return Unauthorized();
            return Ok(new { token });
        }
    }
}
