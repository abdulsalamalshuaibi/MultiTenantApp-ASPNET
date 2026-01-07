namespace MultiTenantApp.Data;

public class Order
{
    public int Id { get; set; }
    public string TenantId { get; set; } = default!;
    public string ProductName { get; set; } = default!;
}
