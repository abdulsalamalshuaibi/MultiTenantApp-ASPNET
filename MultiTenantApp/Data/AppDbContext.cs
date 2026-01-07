using Microsoft.EntityFrameworkCore;
using MultiTenantApp.Tenancy;

namespace MultiTenantApp.Data;

public class AppDbContext : DbContext
{
    public string TenantId { get; }

    public AppDbContext(
        DbContextOptions<AppDbContext> options,
        TenantContext tenantContext)
        : base(options)
    {
        TenantId = tenantContext.TenantId;
    }

    public DbSet<Tenant> Tenants => Set<Tenant>();
    public DbSet<Order> Orders => Set<Order>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>()
            .HasQueryFilter(o => o.TenantId == TenantId);
    }
}
