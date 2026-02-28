namespace eFieldData.DynamicForm.Application.Abstractions;

public interface ITenantContext { int TenantId { get; } string Role { get; } }
