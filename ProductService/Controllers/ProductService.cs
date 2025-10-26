using SharedData;
using SharedData.Models;
using Microsoft.EntityFrameworkCore;

namespace Services;

public class ProductService
{
    private readonly AppDbContext _context;
    private readonly HttpClient _httpClient;

    public ProductService(AppDbContext context, HttpClient httpClient)
    {
        _context = context;
        _httpClient = httpClient;
    }

    public async Task<List<Product>> GetAllProducts()
    {
        return await _context.Products.ToListAsync();
    }

    public async Task<Product?> GetIdProduct(int id)
    {
        return await _context.Products.FirstOrDefaultAsync(p => p.id == id);
    }

    public async Task<Product> AddProduct(Product p)
    {
        _context.Products.Add(p);
        await _context.SaveChangesAsync();
        return p;
    }

    public async Task<Product?> UpdateProduct(int id, Product p)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) return null;

        product.name = p.name;
        product.price = p.price;
        await _context.SaveChangesAsync();

        return p;
    }

    public async Task<bool> DeleteProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) return false;

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return true;
    }

}
