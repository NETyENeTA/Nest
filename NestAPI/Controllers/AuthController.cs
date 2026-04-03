using Microsoft.AspNetCore.Mvc;
using NestAPI.Entities.DTOs;
using NestAPI.Interfaces;

namespace NestAPI.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    public AuthController(IAuthService authService) => _authService = authService;

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest req) =>
        (await _authService.RegisterAsync(req)) is var res && res != null ? Created("", res) : BadRequest("Error");

    [HttpPost("register/manager")]
    public async Task<IActionResult> RegisterManager(RegisterRequest req) =>
        (await _authService.RegisterRoleAsync(req, "Manager")) is var res && res != null ? Created("", res) : BadRequest("Error");

    [HttpPost("register/admin")]
    public async Task<IActionResult> RegisterAdmin(RegisterRequest req) =>
    (await _authService.RegisterRoleAsync(req, "Admin")) is var res && res != null ? Created("", res) : BadRequest("Error");


    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest req) =>
        (await _authService.LoginAsync(req)) is var res && res != null ? Ok(res) : Unauthorized();
}
