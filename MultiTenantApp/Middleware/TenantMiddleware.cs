using MultiTenantApp.Tenancy;

namespace MultiTenantApp.Middleware;

public class TenantMiddleware
{
    private readonly RequestDelegate _next;

    public TenantMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(
        HttpContext context,
        ITenantResolver resolver,
        TenantContext tenantContext)
    {
        var resolved = resolver.Resolve(context);

        if (resolved == null)
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync("Tenant not resolved");
            return;
        }

        tenantContext.TenantId = resolved.TenantId;
        tenantContext.IsDedicated = resolved.IsDedicated;

        await _next(context);
    }
}
