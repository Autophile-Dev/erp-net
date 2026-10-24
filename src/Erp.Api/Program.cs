using Erp.Api;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ErpDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("ErpDatabase")));

var app = builder.Build();
app.MapGet("/api/health", () => Results.Ok(new { status = "ok", service = "Erp.Api", utc = DateTimeOffset.UtcNow }));
app.MapGet("/api/health/database", async (ErpDbContext db, CancellationToken ct) =>
{
    try
    {
        var available = await db.Database.CanConnectAsync(ct);
        return available ? Results.Ok(new { status = "ok", database = "connected" }) :
            Results.Problem("PostgreSQL unavailable", statusCode: 503);
    }
    catch (Exception) { return Results.Problem("Database connection failed", statusCode: 503); }
});
// Deliberately NO unauthenticated business CRUD endpoints. Implement authenticated API + business-scoped policies first.
app.Run();
