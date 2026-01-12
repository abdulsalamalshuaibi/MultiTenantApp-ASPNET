namespace MultiTenantApp.Data;

public class Tenant
{
    public int Id { get; set; }
    public string TenantId { get; set; } = default!;

    public bool IsDedicated { get; set; }
    public string? Domain { get; set; }

    // Used only when IsDedicated = true
    public string? ConnectionString { get; set; }
}
