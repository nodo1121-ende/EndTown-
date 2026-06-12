using EndTown.Data;
using EndTown.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BC = BCrypt.Net.BCrypt;

namespace EndTown.Services
{
    public class AuthService : IAuthService
    {
        private readonly EndTownDbContext _context;
        private readonly IConfiguration _config;

        public AuthService(EndTownDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<AuthResult> RegisterAsync(RegisterRequest request)
        {
            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
                return new AuthResult { Success = false, Message = "Email already exists" };

            if (await _context.Users.AnyAsync(u => u.Username == request.Username))
                return new AuthResult { Success = false, Message = "Username already exists" };

            var passwordHash = BC.HashPassword(request.Password);

            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = passwordHash,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var token = GenerateToken(user);
            return new AuthResult { Success = true, Message = "Registration successful", Token = token };
        }

        public async Task<AuthResult> LoginAsync(LoginRequest request)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null)
                return new AuthResult { Success = false, Message = "Invalid email or password" };

            if (!BC.Verify(request.Password, user.PasswordHash))
                return new AuthResult { Success = false, Message = "Invalid email or password" };

            var token = GenerateToken(user);
            return new AuthResult { Success = true, Message = "Login successful", Token = token };
        }

        private string GenerateToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Username),
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}