# Local ERP Starter — .NET 10 + PostgreSQL

## What this is
A runnable **development starter** for a browser-based .NET ERP environment designed for later on-premises hosting. It includes:
- Blazor Web App dashboard with interactive API and database status checks (http://localhost:5100)
- ASP.NET Core Web API with health endpoints (http://localhost:5101)
- PostgreSQL EF Core models for business, role, permission, user assignment, product, stock movement, sales and journal entry scaffolding
- PowerShell build, launch and publish scripts
- PostgreSQL initial DB script

**Not a finished ERP**: business CRUD, login, permissions enforcement, transactional accounting, audit logs, production migrations, unit tests and WPF desktop wrapper are NOT implemented yet. No business-data endpoint is exposed on purpose.

## Required software (Windows x64)
1. .NET 10 SDK **x64 installer** (not just runtime): https://dotnet.microsoft.com/en-us/download/dotnet/10.0
2. PostgreSQL: https://www.postgresql.org/download/windows/ (pgAdmin optional)
3. Visual Studio with ASP.NET/web development workload OR VS Code + C# extension.
4. Git (optional)

Confirm: `dotnet --version` should display 10.0.xxx; `psql --version` is optional if using pgAdmin.

## PostgreSQL database
Connect as the `postgres` superuser in pgAdmin Query Tool and run (use a NEW password):
```sql
CREATE ROLE erp_app LOGIN PASSWORD 'REPLACE_WITH_STRONG_PASSWORD';
CREATE DATABASE erp_local OWNER erp_app;
```
Run statements separately if pgAdmin requires. Or create role and DB using the GUI.

## API local secret (PowerShell in project root)
```powershell
cd src/Erp.Api
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:ErpDatabase" "Host=localhost;Port=5432;Database=erp_local;Username=erp_app;Password=YOUR_PASSWORD"
cd ../..
```
If `dotnet user-secrets init` says already initialized, continue.

## Create database tables (recommended EF migrations workflow)
```powershell
dotnet tool install --global dotnet-ef --version 10.0.0
dotnet add src/Erp.Api/Erp.Api.csproj package Microsoft.EntityFrameworkCore.Design --version 10.0.0
dotnet ef migrations add InitialCreate --project src/Erp.Api --startup-project src/Erp.Api
dotnet ef database update --project src/Erp.Api --startup-project src/Erp.Api
```
Note: Exact package versions should be compatible with your .NET 10 SDK. Commit generated migrations after review. Do not automatically apply migrations on production startup.

## Build and run
At root in PowerShell:
```powershell
.\scripts\01-verify.ps1
.\scripts\04-build.ps1
```
Open two separate terminals, from project root:
```powershell
.\scripts\02-run-api.ps1
```
```powershell
.\scripts\03-run-web.ps1
```
Then open http://localhost:5100; click Check API and Check Database.
If PowerShell execution policy blocks `.ps1`, run the equivalent `dotnet run --project src/Erp.Api --launch-profile http` and `dotnet run --project src/Erp.Web --launch-profile http` commands directly.

## Deploy to client's on-premises server later
1. Get server OS and hardware details, static LAN IP, access policy, backup destination.
2. Prepare dedicated PostgreSQL with separate least-privileged application DB credentials, and production secrets in environment variables/vault.
3. Generate/review EF Core migrations in development, make DB backups and apply tested deployment scripts.
4. `powershell -File scripts/05-publish.ps1` generates deployable API and Web builds in `artifacts/`.
5. Configure API service using Windows Service or systemd; place behind reverse proxy / TLS; restrict PostgreSQL port to server only.
6. Configure `ApiBaseUrl` for the Web host; configure proper HTTPS endpoints, firewall rules, backups, restores, monitoring and update process.
7. Move reusable Razor components into a Razor Class Library and create `Erp.Desktop` via WPF + BlazorWebView (.NET 10). Desktop will require adapted auth/config, packaging and user acceptance testing.

## Security warning
Currently the dashboard is only a local diagnostic UI, **not a protected financial application**. Before exposing to office network or real customer records, implement verified authentication, per-business authorization for EVERY endpoint, audit trail, safe multi-business isolation, journal balancing/posting, and automated tests. Never treat `businessId` sent by UI as authorization.

## Build verification
Source files were generated in an environment with no `dotnet` executable; running `dotnet build` on your installed .NET SDK is required to validate compilation.
