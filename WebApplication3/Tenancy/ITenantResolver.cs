namespace MultiTenantApp.Tenancy
{
    public interface ITenantResolver
    {
        string? ResolveTenant(HttpContext context);

    }
}
