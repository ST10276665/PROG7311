using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PROG7311.API.Data;
using PROG7311.API.Models;

namespace PROG7311.API.Services
{
    public class AuthService
    {
        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _config;

        public AuthService(ApplicationDbContext db, IConfiguration config)
        {
            _db = db;
            _config = config;
        }

        public async Task<(bool Success, string Message)> RegisterAsync(RegisterRequest request)
        {
            var exists = _db.Users.Any(u => u.Username == request.Username);
            if (exists) return (false, "Username already exists.");

            var hash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            var user = new User
            {
                Username = request.Username,
                PasswordHash = hash,
                Email = request.Email
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            return (true, "User registered.");
        }

        public Task<string?> LoginAsync(LoginRequest request)
        {
            var user = _db.Users.SingleOrDefault(u => u.Username == request.Username);
            if (user == null) return Task.FromResult<string?>(null);

            var valid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
            if (!valid) return Task.FromResult<string?>(null);

            var key = _config["Jwt:Key"]!;
            var issuer = _config["Jwt:Issuer"]!;
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: null,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: creds);

            var tokenStr = new JwtSecurityTokenHandler().WriteToken(token);
            return Task.FromResult<string?>(tokenStr);
        }
    }
}
