namespace eFieldData.DynamicForm.Domain.Entities;

public class Tenant { public int TenantId { get; set; } public string Name { get; set; } = string.Empty; }
public class Form { public int FormId { get; set; } public int TenantId { get; set; } public string Name { get; set; } = string.Empty; public bool IsActive { get; set; } = true; public DateTime CreatedAt { get; set; } = DateTime.UtcNow; public List<FormSection> Sections { get; set; } = []; }
public class FormSection { public int FormSectionId { get; set; } public int FormId { get; set; } public int SortOrder { get; set; } public string Title { get; set; } = "Section"; public List<FormField> Fields { get; set; } = []; }
public class FormField { public int FormFieldId { get; set; } public int FormSectionId { get; set; } public int SortOrder { get; set; } public string Label { get; set; } = string.Empty; public string FieldName { get; set; } = string.Empty; public string FieldType { get; set; } = "text"; public bool IsRequired { get; set; } public string SettingsJson { get; set; } = "{}"; }
public class FormSubmission { public int FormSubmissionId { get; set; } public int FormId { get; set; } public int TenantId { get; set; } public DateTime SubmittedAt { get; set; } = DateTime.UtcNow; public string Status { get; set; } = "Submitted"; public List<FormFieldValue> FieldValues { get; set; } = []; }
public class FormFieldValue { public int FormFieldValueId { get; set; } public int FormSubmissionId { get; set; } public int FormFieldId { get; set; } public string? Value { get; set; } }
