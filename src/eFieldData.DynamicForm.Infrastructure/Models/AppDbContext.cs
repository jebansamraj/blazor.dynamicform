using eFieldData.DynamicForm.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace eFieldData.DynamicForm.Infrastructure.Models;

// Scaffold with Scaffold-DbContext and keep generated file unmodified.
public partial class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Tenant> Tenants => Set<Tenant>();
    public DbSet<Form> Forms => Set<Form>();
    public DbSet<FormSection> FormSections => Set<FormSection>();
    public DbSet<FormField> FormFields => Set<FormField>();
    public DbSet<FormSubmission> FormSubmissions => Set<FormSubmission>();
    public DbSet<FormFieldValue> FormFieldValues => Set<FormFieldValue>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Form>().HasIndex(x => new { x.TenantId, x.Name });
        modelBuilder.Entity<FormSubmission>().HasIndex(x => new { x.TenantId, x.FormId, x.SubmittedAt });
    }
}
