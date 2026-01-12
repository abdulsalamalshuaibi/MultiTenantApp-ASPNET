using Microsoft.AspNetCore.Http;

namespace MultiTenantApp.Tenancy;

public interface ITenantResolver
{
    TenantContext? Resolve(HttpContext context);
}
