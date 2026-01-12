using Microsoft.EntityFrameworkCore;
using MultiTenantApp.Data;
using MultiTenantApp.Middleware;
using MultiTenantApp.Tenancy;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Tenant context (per request)
builder.Services.AddScoped<TenantContext>();

// Tenant resolver
builder.Services.AddScoped<ITenantResolver, HybridTenantResolver>();

// 🔹 Tenant Store DB (ALWAYS shared)
builder.Services.AddDbContext<TenantStoreDbContext>(options =>
    options.UseInMemoryDatabase(
        "TenantStoreDb"));

// 🔹 DbContext factory (Hybrid switch)
builder.Services.AddScoped<DbContextFactory>();

var app = builder.Build();

// 🔹 Seed tenants (first run only)
using (var scope = app.Services.CreateScope())
{
    var store = scope.ServiceProvider.GetRequiredService<TenantStoreDbContext>();
    //store.Database.Migrate();

    if (!store.Tenants.Any())
    {
        store.Tenants.AddRange(
            new Tenant
            {
                TenantId = "tenant1",
                IsDedicated = false,
                Domain = "shop.acme.com"
            },
             new Tenant
             {
                 TenantId = "tenant1",
                 IsDedicated = false,
                 Domain = "customer1.com"
             },
            new Tenant
            {
                TenantId = "tenant3",
                IsDedicated = true,
                ConnectionString = "Server=.;Database=Tenant2Db;Trusted_Connection=True;TrustServerCertificate=True"
            }
        );
        store.SaveChanges();
    }
}

app.UseMiddleware<TenantMiddleware>();
app.MapControllers();
app.Run();
