namespace Controllers;

using Microsoft.AspNetCore.Mvc;
using Models;
using Services;

[ApiController]
[Route("/[controller]")]
public class ProductController : ControllerBase
{
    private readonly ProductService _ProductService;
    public ProductController(ProductService productService)
    {
        _ProductService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        try
        {
            var products = await _ProductService.GetAllProducts();
            return Ok(products);
        }
        catch (Exception e)
        {
            return StatusCode(500, new { error = e.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] Product product)
    {
        try{
            var createdProduct = await _ProductService.AddProduct(product);
            return CreatedAtAction(nameof(CreateProduct), new { id = createdProduct.id }, createdProduct);
        }
        catch (Exception e){
            return StatusCode(500, new { error = e.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProduct([FromRoute] int id)
    {
        var p = await _ProductService.GetIdProduct(id);
        return Ok(p);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct([FromBody] Product p, [FromRoute] int id)
    {
        try
        {
            var product = await _ProductService.UpdateProduct(id, p);
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
            var deleted = await _ProductService.DeleteProduct(id);
            if (!deleted) return NotFound(new { message="Product deleted succesfully"});
            else return Ok(new{ message= "Product Not Found"});
        }catch(Exception e)
        {
            return StatusCode(500, new { error= e.Message});
        }
    }
}