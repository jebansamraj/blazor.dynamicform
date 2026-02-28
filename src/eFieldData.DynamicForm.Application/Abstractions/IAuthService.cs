using eFieldData.DynamicForm.Application.DTOs;

namespace eFieldData.DynamicForm.Application.Abstractions;

public interface IAuthService { Task<LoginResponse?> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default); }
