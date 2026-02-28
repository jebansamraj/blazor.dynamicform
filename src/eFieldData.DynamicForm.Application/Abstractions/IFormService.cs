using eFieldData.DynamicForm.Application.DTOs;

namespace eFieldData.DynamicForm.Application.Abstractions;

public interface IFormService
{
    Task<IReadOnlyList<FormDto>> GetFormsAsync(CancellationToken cancellationToken = default);
    Task<FormDto?> GetFormAsync(int formId, CancellationToken cancellationToken = default);
    Task<FormDto> CreateFormAsync(FormUpsertRequest request, CancellationToken cancellationToken = default);
    Task<FormDto?> UpdateFormAsync(int formId, FormUpsertRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteFormAsync(int formId, CancellationToken cancellationToken = default);
    Task<int?> SubmitFormAsync(int formId, SubmitFormRequest request, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SubmissionSummaryDto>> GetSubmissionsAsync(int formId, int page, int pageSize, string? status, DateTime? fromDate, DateTime? toDate, CancellationToken cancellationToken = default);
    Task<SubmissionDetailDto?> GetSubmissionAsync(int submissionId, CancellationToken cancellationToken = default);
}
