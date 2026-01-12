using Microsoft.EntityFrameworkCore;
using MultiTenantApp.Tenancy;

namespace MultiTenantApp.Data;

public class AppDbContext : DbContext
{
    private readonly TenantContext _tenant;

    public AppDbContext(
        DbContextOptions<AppDbContext> options,
        TenantContext tenant)
        : base(options)
    {
        _tenant = tenant;
    }

    public DbSet<Order> Orders => Set<Order>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        if (!_tenant.IsDedicated)
        {
            modelBuilder.Entity<Order>()
                .HasQueryFilter(o => o.TenantId == _tenant.TenantId);
        }
    }
}
