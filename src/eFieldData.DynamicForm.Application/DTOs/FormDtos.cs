namespace eFieldData.DynamicForm.Application.DTOs;

public record FieldDto(int Id, string Label, string FieldName, string FieldType, bool IsRequired, int SortOrder, string SettingsJson);
public record SectionDto(int Id, string Title, int SortOrder, List<FieldDto> Fields);
public record FormDto(int Id, string Name, bool IsActive, List<SectionDto> Sections);
public record FormUpsertRequest(string Name, bool IsActive, List<SectionDto> Sections);
public record SubmissionFieldValueDto(int FormFieldId, string? Value);
public record SubmitFormRequest(List<SubmissionFieldValueDto> Values);
public record SubmissionSummaryDto(int Id, DateTime SubmittedAt, string Status);
public record SubmissionDetailDto(int Id, int FormId, DateTime SubmittedAt, string Status, Dictionary<int, string?> Values);
