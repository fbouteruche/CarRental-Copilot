using CarRental.BlazorApp.Components;
using CarRental.BlazorApp.Services;
using CarRental.Controllers.CustomersModule;
using CarRental.Controllers.VehicleModule;
using System.Configuration;
using System.Runtime.Versioning;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Configure services for dependency injection
builder.Services.AddScoped<CustomerController>();
// Only register VehicleController on Windows platforms
if (OperatingSystem.IsWindows())
{
    builder.Services.AddScoped<VehicleController>();
}

// Initialize configuration adapter for legacy code
ConfigurationAdapter.Initialize(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
