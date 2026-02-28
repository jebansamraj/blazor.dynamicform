using eFieldData.DynamicForm.Application.Abstractions;
using eFieldData.DynamicForm.Application.DTOs;
using eFieldData.DynamicForm.Domain.Entities;
using eFieldData.DynamicForm.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace eFieldData.DynamicForm.Infrastructure.Services;

public class FormService(AppDbContext dbContext, ITenantContext tenantContext) : IFormService
{
    public async Task<FormDto> CreateFormAsync(FormUpsertRequest request, CancellationToken cancellationToken = default)
    {
        var form = new Form { Name = request.Name, IsActive = request.IsActive, TenantId = tenantContext.TenantId };
        form.Sections = request.Sections.Select(s => new FormSection
        {
            Title = s.Title, SortOrder = s.SortOrder,
            Fields = s.Fields.Select(f => new FormField { Label = f.Label, FieldName = f.FieldName, FieldType = f.FieldType, IsRequired = f.IsRequired, SortOrder = f.SortOrder, SettingsJson = f.SettingsJson }).ToList()
        }).ToList();
        dbContext.Forms.Add(form);
        await dbContext.SaveChangesAsync(cancellationToken);
        return await GetFormAsync(form.FormId, cancellationToken) ?? new FormDto(form.FormId, form.Name, form.IsActive, []);
    }

    public async Task<bool> DeleteFormAsync(int formId, CancellationToken cancellationToken = default)
    {
        var form = await dbContext.Forms.FirstOrDefaultAsync(x => x.FormId == formId && x.TenantId == tenantContext.TenantId, cancellationToken);
        if (form is null) return false;
        dbContext.Forms.Remove(form);
        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<FormDto?> GetFormAsync(int formId, CancellationToken cancellationToken = default)
    {
        var form = await dbContext.Forms.Include(x => x.Sections).ThenInclude(x => x.Fields).FirstOrDefaultAsync(x => x.FormId == formId && x.TenantId == tenantContext.TenantId, cancellationToken);
        return form is null ? null : MapForm(form);
    }

    public async Task<IReadOnlyList<FormDto>> GetFormsAsync(CancellationToken cancellationToken = default)
        => await dbContext.Forms.Where(x => x.TenantId == tenantContext.TenantId).Include(x => x.Sections).ThenInclude(x => x.Fields).Select(x => MapForm(x)).ToListAsync(cancellationToken);

    public async Task<SubmissionDetailDto?> GetSubmissionAsync(int submissionId, CancellationToken cancellationToken = default)
    {
        var entity = await dbContext.FormSubmissions.Include(x => x.FieldValues).FirstOrDefaultAsync(x => x.FormSubmissionId == submissionId && x.TenantId == tenantContext.TenantId, cancellationToken);
        return entity is null ? null : new SubmissionDetailDto(entity.FormSubmissionId, entity.FormId, entity.SubmittedAt, entity.Status, entity.FieldValues.ToDictionary(x => x.FormFieldId, x => x.Value));
    }

    public async Task<IReadOnlyList<SubmissionSummaryDto>> GetSubmissionsAsync(int formId, int page, int pageSize, string? status, DateTime? fromDate, DateTime? toDate, CancellationToken cancellationToken = default)
    {
        var query = dbContext.FormSubmissions.Where(x => x.TenantId == tenantContext.TenantId && x.FormId == formId);
        if (!string.IsNullOrWhiteSpace(status)) query = query.Where(x => x.Status == status);
        if (fromDate.HasValue) query = query.Where(x => x.SubmittedAt >= fromDate.Value);
        if (toDate.HasValue) query = query.Where(x => x.SubmittedAt <= toDate.Value);

        return await query.OrderByDescending(x => x.SubmittedAt).Skip((page - 1) * pageSize).Take(pageSize).Select(x => new SubmissionSummaryDto(x.FormSubmissionId, x.SubmittedAt, x.Status)).ToListAsync(cancellationToken);
    }

    public async Task<int?> SubmitFormAsync(int formId, SubmitFormRequest request, CancellationToken cancellationToken = default)
    {
        var formExists = await dbContext.Forms.AnyAsync(x => x.FormId == formId && x.TenantId == tenantContext.TenantId, cancellationToken);
        if (!formExists) return null;

        var submission = new FormSubmission { FormId = formId, TenantId = tenantContext.TenantId, FieldValues = request.Values.Select(x => new FormFieldValue { FormFieldId = x.FormFieldId, Value = x.Value }).ToList() };
        dbContext.FormSubmissions.Add(submission);
        await dbContext.SaveChangesAsync(cancellationToken);
        return submission.FormSubmissionId;
    }

    public async Task<FormDto?> UpdateFormAsync(int formId, FormUpsertRequest request, CancellationToken cancellationToken = default)
    {
        var entity = await dbContext.Forms.Include(x => x.Sections).ThenInclude(x => x.Fields).FirstOrDefaultAsync(x => x.FormId == formId && x.TenantId == tenantContext.TenantId, cancellationToken);
        if (entity is null) return null;
        entity.Name = request.Name;
        entity.IsActive = request.IsActive;
        dbContext.FormSections.RemoveRange(entity.Sections);
        entity.Sections = request.Sections.Select(s => new FormSection
        {
            Title = s.Title, SortOrder = s.SortOrder,
            Fields = s.Fields.Select(f => new FormField { Label = f.Label, FieldName = f.FieldName, FieldType = f.FieldType, IsRequired = f.IsRequired, SortOrder = f.SortOrder, SettingsJson = f.SettingsJson }).ToList()
        }).ToList();
        await dbContext.SaveChangesAsync(cancellationToken);
        return MapForm(entity);
    }

    private static FormDto MapForm(Form form) => new(form.FormId, form.Name, form.IsActive,
        form.Sections.OrderBy(x => x.SortOrder).Select(s => new SectionDto(s.FormSectionId, s.Title, s.SortOrder,
            s.Fields.OrderBy(f => f.SortOrder).Select(f => new FieldDto(f.FormFieldId, f.Label, f.FieldName, f.FieldType, f.IsRequired, f.SortOrder, f.SettingsJson)).ToList())).ToList());
}
