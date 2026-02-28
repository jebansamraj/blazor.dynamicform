using eFieldData.DynamicForm.Application.Abstractions;
using eFieldData.DynamicForm.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace eFieldData.DynamicForm.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var response = await authService.LoginAsync(request, cancellationToken);
        return response is null ? Unauthorized(new { message = "Invalid credentials" }) : Ok(response);
    }
}
