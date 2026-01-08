using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;
using TaskApi.Models;
using Microsoft.IdentityModel.Tokens;

namespace TaskApi.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private static readonly List<User> users = new();
        private const string JwtKey = "your-secret-key-change-this-in-production-at-least-32-chars!";

        [HttpPost("register")]
        public ActionResult<AuthResponse> Register([FromBody] RegisterRequest req)
        {
            if (users.Any(u => u.Email == req.Email))
                return BadRequest("Email already exists");

            var user = new User
            {
                Email = req.Email,
                Name = req.Name,
                PasswordHash = HashPassword(req.Password),
                CreatedAt = DateTime.UtcNow
            };

            // Auto-assign ID
            user.Id = users.Count > 0 ? users.Max(u => u.Id) + 1 : 1;
            users.Add(user);

            var token = GenerateJwt(user.Id, user.Email);
            return Ok(new AuthResponse { Token = token, UserId = user.Id, Email = user.Email });
        }

        [HttpPost("login")]
        public ActionResult<AuthResponse> Login([FromBody] LoginRequest req)
        {
            var user = users.FirstOrDefault(u => u.Email == req.Email);
            if (user == null || !VerifyPassword(req.Password, user.PasswordHash))
                return Unauthorized("Invalid email or password");

            var token = GenerateJwt(user.Id, user.Email);
            return Ok(new AuthResponse { Token = token, UserId = user.Id, Email = user.Email });
        }

        private string GenerateJwt(int userId, string email)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "TaskApi",
                audience: "TaskApiUsers",
                claims: new[]
                {
                    new System.Security.Claims.Claim("sub", userId.ToString()),
                    new System.Security.Claims.Claim("email", email)
                },
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hash);
            }
        }

        private bool VerifyPassword(string password, string hash)
        {
            var hashOfInput = HashPassword(password);
            return hashOfInput == hash;
        }
    }

    public class RegisterRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }

    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class AuthResponse
    {
        public string Token { get; set; } = string.Empty;
        public int UserId { get; set; }
        public string Email { get; set; } = string.Empty;
    }
}
