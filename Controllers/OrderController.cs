namespace Controllers;

using Microsoft.AspNetCore.Mvc;
using Models;
using Services;

[ApiController]
[Route("/[controller]")]
public class OrderController : ControllerBase
{
    private readonly OrderService _OrderService;

    public OrderController(OrderService orderService)
    {
        _OrderService = orderService;
    }

    [HttpGet]
    public async Task<IActionResult> GetOrders()
    {
        try
        {
            var orders = await _OrderService.GetAllOrders();
            return Ok(orders);
        }
        catch (Exception e)
        {
            return StatusCode(500, new { error = e.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] Order order)
    {
        try
        {
            var o = await _OrderService.PostOrder(order);
            return CreatedAtAction(nameof(CreateOrder), new { id = o.id }, o);
        }catch(Exception e)
        {
            return StatusCode(500, new { error = e.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrder([FromRoute] int id)
    {
        try
        {
            Order o = await _OrderService.GetIdOrder(id);
            return Ok(o);
        }
        catch (Exception e)
        {
            return StatusCode(500, new { error = e.Message });
        }
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct([FromBody] Order p, [FromRoute] int id)
    {
        try
        {
            var product = await _OrderService.UpdateOrder(id, p);
            if (product == null) return NotFound(new { message = "Product not found" });
            return Ok(product);
        }
        catch (Exception e)
        {
            return StatusCode(500, new { error = e.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct([FromRoute] int id)
    {
        try
        {
            var deleted = await _OrderService.DeleteOrder(id);
            if (!deleted) return NotFound(new { message="Product deleted succesfully"});
            else return Ok(new{ message= "Product Not Found"});
        }catch(Exception e)
        {
            return StatusCode(500, new { error= e.Message});
        }
    }
}