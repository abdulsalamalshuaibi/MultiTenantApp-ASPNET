using Microsoft.EntityFrameworkCore;

namespace MultiTenantApp.Data;

public class TenantStoreDbContext : DbContext
{
    public TenantStoreDbContext(DbContextOptions<TenantStoreDbContext> options)
        : base(options)
    {
    }

    public DbSet<Tenant> Tenants => Set<Tenant>();
}