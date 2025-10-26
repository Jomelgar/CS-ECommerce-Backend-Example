using Microsoft.AspNetCore.Mvc;
using SharedData.Models;
using Services;

namespace Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly ProductService _productService;

    public ProductController(ProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        var products = await _productService.GetAllProducts();
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProduct(int id)
    {
        var p = await _productService.GetIdProduct(id);
        if (p == null) return NotFound(new { message = "Product not found" });
        return Ok(p);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] Product product)
    {
        var created = await _productService.AddProduct(product);
        return CreatedAtAction(nameof(GetProduct), new { id = created.id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct([FromBody] Product p, [FromRoute] int id)
    {
        var updated = await _productService.UpdateProduct(id, p);
        if (updated == null) return NotFound(new { message = "Product not found" });
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct([FromRoute] int id)
    {
        var deleted = await _productService.DeleteProduct(id);
        if (!deleted) return NotFound(new { message = "Product not found" });
        return Ok(new { message = "Product deleted successfully" });
    }
}
