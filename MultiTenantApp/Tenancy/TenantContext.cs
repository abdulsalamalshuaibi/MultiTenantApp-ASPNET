namespace MultiTenantApp.Tenancy;

public class TenantContext
{
    public string TenantId { get; set; } = default!;
    public bool IsDedicated { get; set; }
    public string? ConnectionString { get; set; }
}
