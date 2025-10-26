using Microsoft.AspNetCore.Mvc;
using SharedData.Models;
using Services;

namespace Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly OrderService _orderService;

    public OrderController(OrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    public async Task<IActionResult> GetOrders()
    {
        var orders = await _orderService.GetAllOrders();
        return Ok(orders);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrder(int id)
    {
        var o = await _orderService.GetIdOrder(id);
        if (o == null) return NotFound(new { message = "Order not found" });
        return Ok(o);
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] Order order)
    {
        var o = await _orderService.PostOrder(order);
        return CreatedAtAction(nameof(GetOrder), new { id = o.id }, o);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateOrder([FromBody] Order o, int id)
    {
        var updated = await _orderService.UpdateOrder(id, o);
        if (updated == null) return NotFound(new { message = "Order not found" });
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrder(int id)
    {
        var deleted = await _orderService.DeleteOrder(id);
        if (!deleted) return NotFound(new { message = "Order not found" });
        return Ok(new { message = "Order deleted successfully" });
    }
}
