using System.Security.Claims;
using eFieldData.DynamicForm.Application.Abstractions;
using Microsoft.AspNetCore.Http;

namespace eFieldData.DynamicForm.Infrastructure.Security;

public class TenantContext(IHttpContextAccessor accessor) : ITenantContext
{
    public int TenantId => int.TryParse(accessor.HttpContext?.User.FindFirstValue("tenantId"), out var tenantId) ? tenantId : 0;
    public string Role => accessor.HttpContext?.User.FindFirstValue(ClaimTypes.Role) ?? "User";
}
