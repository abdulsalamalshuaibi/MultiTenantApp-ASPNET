using Microsoft.EntityFrameworkCore;
using MultiTenantApp.Data;

namespace MultiTenantApp.Tenancy;

public class HybridTenantResolver : ITenantResolver
{
    private readonly TenantStoreDbContext _store;
    private const string AppRootDomain = "myapp.com";
    private Tenant? tenant;

    public HybridTenantResolver(TenantStoreDbContext store)
    {
        _store = store;
    }

    public TenantContext? Resolve(HttpContext context)
    {
        var host = context.Request.Host.Host.ToLowerInvariant();

        if (host.StartsWith("www."))
            host = host.Substring(4);
        

        tenant = _store.Tenants
          .AsNoTracking()
          .FirstOrDefault(t => t.Domain == host);


        if (host.EndsWith(AppRootDomain))
        {
            var parts = host.Split('.');
            if (parts.Length < 3)
                return null;

            var tenantId = parts[0];

            tenant = _store.Tenants
               .FirstOrDefault(t => t.TenantId == tenantId);
        }

        if (tenant == null)
            return null;

        return new TenantContext
        {
            TenantId = tenant.TenantId,
            IsDedicated = tenant.IsDedicated,
            ConnectionString = tenant.ConnectionString
        };
    }
}
