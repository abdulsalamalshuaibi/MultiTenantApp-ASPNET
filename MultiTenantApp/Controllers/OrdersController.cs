using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using MultiTenantApp.Data;
using MultiTenantApp.Tenancy;

[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    private readonly DbContextFactory _factory;
    private readonly TenantContext _tenant;

    public OrdersController(DbContextFactory factory, TenantContext tenant)
    {
        _factory = factory;
        _tenant = tenant;
    }

    [HttpGet]
    public IActionResult Get()
    {
        using var db = _factory.CreateDbContext(_tenant);

        return Ok(new
        {
            Tenant = _tenant.TenantId,
            Mode = _tenant.IsDedicated ? "Dedicated DB" : "Shared DB",
            Orders = db.Orders.ToList()
        });
    }
}
