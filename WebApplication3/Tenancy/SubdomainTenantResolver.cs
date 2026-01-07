using Microsoft.AspNetCore.Http;

namespace MultiTenantApp.Tenancy;

public class SubdomainTenantResolver : ITenantResolver
{
    public string? ResolveTenant(HttpContext context)
    {
        var host = context.Request.Host.Host;
        // tenant1.myapp.com

        var parts = host.Split('.', StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length < 3)
            return null;

        var tenant = parts[0].ToLowerInvariant();

        // ignore reserved subdomains
        if (tenant == "www" || tenant == "app")
            return null;

        return tenant;
    }
}
