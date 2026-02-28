# eFieldData.DynamicForm

Enterprise-ready multi-tenant dynamic form builder and field data collection platform with .NET 8 API + React TypeScript frontend.

## Prerequisites
- .NET 8 SDK
- SQL Server (local or Azure SQL)
- Node.js 20+
- npm 10+

## Solution Structure
- `eFieldData.DynamicForm.sln`
- `src/eFieldData.DynamicForm.API`
- `src/eFieldData.DynamicForm.Application`
- `src/eFieldData.DynamicForm.Infrastructure`
- `src/eFieldData.DynamicForm.Domain`
- `frontend/efielddata-dynamicform`

## Database Creation Steps
1. Create database:
   ```sql
   CREATE DATABASE eFieldDataDynamicFormDb;
   ```
2. Create tables: `Tenants`, `Forms`, `FormSections`, `FormFields`, `FieldSettings`, `FormSubmissions`, `FormFieldValues` with tenant FK and EAV values table.
3. Add indexes on tenant-scoped and submission query columns.

## DB First Scaffold Steps
Run inside `src/eFieldData.DynamicForm.Infrastructure`:
```bash
Scaffold-DbContext "Server=.;Database=eFieldDataDynamicFormDb;Trusted_Connection=True;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -Context AppDbContext -Force
```
Use partial classes for custom extensions and never modify scaffold-generated files.

## Backend Run Steps
1. Update `src/eFieldData.DynamicForm.API/appsettings.json` connection string.
2. Run:
   ```bash
   dotnet restore
   dotnet build eFieldData.DynamicForm.sln
   dotnet run --project src/eFieldData.DynamicForm.API
   ```
3. API base URL: `http://localhost:5000`.

## Frontend Run Steps
1. Go to frontend:
   ```bash
   cd frontend/efielddata-dynamicform
   npm install
   npm run dev
   ```
2. Set optional `.env`:
   ```bash
   VITE_API_URL=http://localhost:5000/api
   ```

## Environment Variables
### Backend (`appsettings`)
- `ConnectionStrings:DefaultConnection`
- `Jwt:Key`
- `Jwt:Issuer`
- `Jwt:Audience`

### Frontend (`.env`)
- `VITE_API_URL`

## Default Admin Login
- Username: `admin@tenant1.com`
- Password: `Admin@123`

## How to Test Dynamic Form
1. Login.
2. Open Dashboard.
3. Create form.
4. Add fields with drag/drop.
5. Edit field settings and options JSON (`{"options":["A","B"]}`).
6. Save and verify redirect to dashboard.
7. Open form view and submit values.
8. Open submissions list and detail pages.


## Windows Path Length Troubleshooting
This repository includes `Directory.Build.props` that redirects `obj`/`bin` to short root-level folders (`.b/obj` and `.b/bin`) to avoid `MSBuild` path-length failures in deep directories.

If you still get path length errors:
1. Move the repo to a shorter path (example: `C:\src\eFieldData.DynamicForm`).
2. Enable long paths in Windows policy/registry.
3. Clean and rebuild:
   ```bash
   dotnet clean eFieldData.DynamicForm.sln
   dotnet build eFieldData.DynamicForm.sln
   ```

## Production Build Steps
- Backend:
  ```bash
  dotnet publish src/eFieldData.DynamicForm.API -c Release -o ./artifacts/api
  ```
- Frontend:
  ```bash
  cd frontend/efielddata-dynamicform
  npm run build
  ```

## Azure Readiness Notes
- JWT auth with tenant claim + role checks.
- Tenant isolation enforced server-side per request.
- Pagination endpoint for submissions.
- SQL indexes configured in model.
- Stateless API for horizontal scaling.
