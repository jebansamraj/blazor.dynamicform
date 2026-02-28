using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using eFieldData.DynamicForm.Application.Abstractions;
using eFieldData.DynamicForm.Application.DTOs;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace eFieldData.DynamicForm.Infrastructure.Services;

public class AuthService(IConfiguration configuration) : IAuthService
{
    public Task<LoginResponse?> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var users = new Dictionary<string, (string Password, string Role, int TenantId)>
        {
            ["admin@tenant1.com"] = ("Admin@123", "Admin", 1),
            ["user@tenant1.com"] = ("User@123", "User", 1),
            ["admin@tenant2.com"] = ("Admin@123", "Admin", 2)
        };

        if (!users.TryGetValue(request.Username.ToLowerInvariant(), out var user) || user.Password != request.Password)
        {
            return Task.FromResult<LoginResponse?>(null);
        }

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, request.Username),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim("tenantId", user.TenantId.ToString())
        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"] ?? "super-secret-dev-key-12345"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(issuer: configuration["Jwt:Issuer"], audience: configuration["Jwt:Audience"], claims: claims, expires: DateTime.UtcNow.AddHours(8), signingCredentials: creds);

        return Task.FromResult<LoginResponse?>(new LoginResponse(new JwtSecurityTokenHandler().WriteToken(token), request.Username, user.Role, user.TenantId));
    }
}
