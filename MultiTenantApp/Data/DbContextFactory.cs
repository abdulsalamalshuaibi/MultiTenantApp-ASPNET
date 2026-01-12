using Microsoft.EntityFrameworkCore;
using MultiTenantApp.Tenancy;

namespace MultiTenantApp.Data;

public class DbContextFactory
{
    private readonly IConfiguration _config;
    private readonly TenantContext _tenant;


    public DbContextFactory(IConfiguration config, TenantContext tenant)
    {
        _config = config;
        _tenant = tenant;
    }

    public AppDbContext CreateDbContext(TenantContext tenant)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>();

        if (tenant.IsDedicated && !string.IsNullOrWhiteSpace(tenant.ConnectionString))
        {
            // 🔐 Dedicated DB
            options.UseInMemoryDatabase(tenant.ConnectionString);
        }
        else
        {
            // 🗄️ Shared DB
            options.UseInMemoryDatabase(
                "SharedDatabase");
        }

        return new AppDbContext(options.Options, tenant);
    }
}
