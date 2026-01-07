using Microsoft.AspNetCore.Http;
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
        ITenantResolver tenantResolver,
        TenantContext tenantContext)
    {
        var tenantId = tenantResolver.ResolveTenant(context);

        if (string.IsNullOrWhiteSpace(tenantId))
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync("Tenant not found");
            return;
        }

        tenantContext.TenantId = tenantId;

        await _next(context);
    }
}
