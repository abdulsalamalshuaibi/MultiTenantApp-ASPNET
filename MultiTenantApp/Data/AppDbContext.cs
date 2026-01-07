using Microsoft.EntityFrameworkCore;
using MultiTenantApp.Tenancy;

namespace MultiTenantApp.Data;

public class AppDbContext : DbContext
{
    private readonly TenantContext _tenantContext;

    public AppDbContext(
        DbContextOptions<AppDbContext> options,
        TenantContext tenantContext)
        : base(options)
    {
        _tenantContext = tenantContext;
    }

    public DbSet<Order> Orders => Set<Order>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>()
            .HasQueryFilter(o => o.TenantId == _tenantContext.TenantId);
    }
}
