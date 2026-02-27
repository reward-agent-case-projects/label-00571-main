using Dqsm.Backend.Data;
using Dqsm.Backend.Filters;
using Dqsm.Backend.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// Add Controllers with Views and register Global Filter
builder.Services.AddControllersWithViews(options =>
{
    // Global Filter Registration
    options.Filters.Add<GlobalAuditLogFilter>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=WeChat}/{action=Index}/{id?}");

// Ensure Database Created FIRST (Code First) — must succeed before serving requests
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    context.Database.EnsureCreated(); // Creates DB and Tables if not exist
}

// Initialize Static Log Service AFTER database is ready
StaticLogService.Initialize(app.Services);

// Log startup (now safe because tables exist)
StaticLogService.Info("System", "Application started, database initialized.");

app.Run();
