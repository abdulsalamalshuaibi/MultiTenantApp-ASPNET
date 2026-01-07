using Microsoft.AspNetCore.Mvc;
using MultiTenantApp.Data;
using MultiTenantApp.Tenancy;

namespace MultiTenantApp.Controllers;

[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly TenantContext _tenantContext;

    public OrdersController(AppDbContext db, TenantContext tenantContext)
    {
        _db = db;
        _tenantContext = tenantContext;
    }

    [HttpGet]
    public IActionResult Get()
    {
        var orders = _db.Orders.ToList();
        return Ok(new
        {
            Tenant = _tenantContext.TenantId,
            Data = orders
        });
    }

    [HttpPost]
    public IActionResult Create([FromBody] CreateOrderRequest request)
    {
        _db.Orders.Add(new Order
        {
            ProductName = request.ProductName,
            TenantId = _tenantContext.TenantId
        });

        _db.SaveChanges();
        return Ok();
    }

}
