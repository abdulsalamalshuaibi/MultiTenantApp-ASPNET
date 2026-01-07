using Microsoft.EntityFrameworkCore;
using MultiTenantApp.Data;

namespace MultiTenantApp.Tenancy;

public class HybridTenantResolver : ITenantResolver
{
    private readonly AppDbContext _db;
    private const string AppRootDomain = "myapp.com";

    public HybridTenantResolver(AppDbContext db)
    {
        _db = db;
    }

    public string? ResolveTenant(HttpContext context)
    {
        var host = context.Request.Host.Host.ToLowerInvariant();

        // 1️⃣ Try custom domain
        var tenant = _db.Tenants
            .AsNoTracking()
            .FirstOrDefault(t => t.Domain == host);

        if (tenant != null)
            return tenant.TenantId;

        // 2️⃣ Fallback to subdomain
        if (host.EndsWith(AppRootDomain))
        {
            var parts = host.Split('.', StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length >= 3)
            {
                var subdomain = parts[0];

                if (subdomain != "www" && subdomain != "app")
                    return subdomain; // tenant id
            }
        }

        // 3️⃣ Tenant not found
        return null;
    }
}
