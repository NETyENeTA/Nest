using NestAPI.Entities.DTOs;

namespace NestAPI.Interfaces;

public interface IAuthService
{
    Task<UserResponse?> RegisterAsync(RegisterRequest request);
    Task<UserResponse?> RegisterRoleAsync(RegisterRequest request, string role);
    Task<AuthResponse?> LoginAsync(LoginRequest request);
}