using CarRental.BlazorApp.Components;
using CarRental.Controllers.CustomersModule;
using CarRental.Controllers.VehicleModule;
using CarRental.Controllers.EmployeeModule;
using CarRental.Controllers.ServiceModule;
using CarRental.Controllers.CouponModule;
using CarRental.Controllers.RentalModule;
using CarRental.Controllers.Shared;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Configure services for dependency injection
builder.Services.AddScoped<CustomerController>();
builder.Services.AddScoped<VehicleController>();
builder.Services.AddScoped<EmployeeController>();
builder.Services.AddScoped<ServiceController>();
builder.Services.AddScoped<CouponController>();
builder.Services.AddScoped<RentalController>();

// Configure database configuration
builder.Services.AddSingleton<IDatabaseConfiguration>(provider =>
{
    var config = provider.GetRequiredService<IConfiguration>();
    return new DatabaseConfiguration(config);
});

// Initialize database configuration for the legacy static Db class
var tempConfig = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .Build();

var dbConfig = new DatabaseConfiguration(tempConfig);
Db.Initialize(dbConfig);

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

// Make the Program class accessible for testing
public partial class Program { }
