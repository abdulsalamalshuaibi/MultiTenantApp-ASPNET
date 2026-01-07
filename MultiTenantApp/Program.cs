using Microsoft.EntityFrameworkCore;
using MultiTenantApp.Data;
using MultiTenantApp.Middleware;
using MultiTenantApp.Tenancy;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();

// Tenancy
builder.Services.AddScoped<TenantContext>();
builder.Services.AddScoped<ITenantResolver, HybridTenantResolver>();


// EF Core (example: InMemory)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("MultiTenantDb"));

var app = builder.Build();

// Seed tenants (demo)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();

    if (!db.Tenants.Any())
    {
        db.Tenants.AddRange(
            new Tenant { TenantId = "tenant1", Domain = "customer1.com" },
            new Tenant { TenantId = "tenant2", Domain = "shop.acme.com" },
            new Tenant { TenantId = "tenant3", Domain = "tenant3.myapp.com" }
        );
        db.SaveChanges();
    }
}

// 🔥 Tenant must be resolved FIRST
app.UseMiddleware<TenantMiddleware>();

app.MapControllers();

app.Run();
