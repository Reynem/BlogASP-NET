using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (request.Username == "admin" && request.Password == "password")
            {
                return Ok(new { Token = "fake-jwt-token" });
            }
            return Unauthorized();
        }
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterRequest request)
        {
            return CreatedAtAction(nameof(Register), new { Id = 1, Username = request.Username });
        }



        public class LoginRequest
        {
            public required string Username { get; set; }
            public required string Password { get; set; }
        }

        public class RegisterRequest
        {
            public required string Username { get; set; }
            public required string Email { get; set; }
            public required string Password { get; set; }
        }
    }
}
