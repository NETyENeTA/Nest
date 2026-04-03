using NestAPI.Entities.DTOs;

namespace NestAPI.Interfaces;

public interface IAuthService
{
    Task<UserResponse?> RegisterAsync(RegisterRequest request);
    Task<AuthResponse?> LoginAsync(LoginRequest request);
}