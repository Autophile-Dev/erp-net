using Erp.Web.Components;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorComponents().AddInteractiveServerComponents();
builder.Services.AddHttpClient("erp-api", client =>
    client.BaseAddress = new Uri(builder.Configuration["ApiBaseUrl"] ?? "http://localhost:5101"));
var app = builder.Build();
if (!app.Environment.IsDevelopment()) { app.UseExceptionHandler("/Error"); app.UseHsts(); }
app.UseHttpsRedirection();
app.UseAntiforgery();
app.MapStaticAssets();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();
app.Run();
