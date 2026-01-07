using Microsoft.EntityFrameworkCore;
using MultiTenantApp.Data;
using MultiTenantApp.Middleware;
using MultiTenantApp.Tenancy;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// Tenancy
builder.Services.AddScoped<TenantContext>();
builder.Services.AddScoped<ITenantResolver, SubdomainTenantResolver>();

// EF Core (example: InMemory)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("MultiTenantDb"));

var app = builder.Build();

// 🔥 Tenant must be resolved FIRST
app.UseMiddleware<TenantMiddleware>();

app.MapControllers();

app.Run();
