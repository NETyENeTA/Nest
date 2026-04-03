using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NestAPI.Data;
using NestAPI.Entities;
using NestAPI.Entities.DTOs;
using NestAPI.Interfaces;

namespace NestAPI.Services;

public class AuthService : IAuthService
{
    private readonly DateBaseContext _db;
    private readonly IConfiguration _config;
    public AuthService(DateBaseContext db, IConfiguration config) { _db = db; _config = config; }

    public async Task<UserResponse?> RegisterAsync(RegisterRequest request)
    {
        if (await _db.Users.AnyAsync(u => u.Username == request.Username)) return null;
        var user = new User { Username = request.Username, PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password) };
        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        return new UserResponse { Username = user.Username, Role = user.Role };
    }

    public async Task<AuthResponse?> LoginAsync(LoginRequest request)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash)) return null;

        var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
        var key = System.Text.Encoding.ASCII.GetBytes(_config["Jwt:Key"]!);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new System.Security.Claims.ClaimsIdentity(new[] {
                new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.NameIdentifier, user.Id),
                new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Role, user.Role)
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        return new AuthResponse { Token = tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor)), Role = user.Role, ExpiresIn = 3600 };
    }
}