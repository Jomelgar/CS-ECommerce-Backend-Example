using Microsoft.EntityFrameworkCore;
using SharedData;
using SharedData.Models;
using System.Net.Http.Json;

namespace Services;

public class InventoryService
{
    private readonly AppDbContext _context;
    private readonly HttpClient _httpClient;

    public InventoryService(AppDbContext context, HttpClient httpClient)
    {
        _context = context;
        _httpClient = httpClient;
    }

    // Obtener todos los inventarios
    public async Task<List<Inventory>> GetAllInventoriesAsync()
    {
        return await _context.Inventories.AsNoTracking().ToListAsync();
    }

    // Obtener uno
    public async Task<Inventory?> GetInventoryByIdAsync(int id)
    {
        return await _context.Inventories.AsNoTracking().FirstOrDefaultAsync(i => i.id == id);
    }

    // Crear nuevo inventario (valida que el producto exista en otro microservicio)
    public async Task<(bool success, string message, Inventory? inventory)> AddInventoryAsync(Inventory dto)
    {
        // Verificar si el producto existe en ProductService
        var productResponse = await _httpClient.GetAsync($"http://localhost:5075/api/product/{dto.id_product}");

        if (!productResponse.IsSuccessStatusCode)
            return (false, "Product not found in ProductService", null);

        var entity = new Inventory
        {
            id_product = dto.id_product,
            total_amount = dto.total_amount
        };

        _context.Inventories.Add(entity);
        await _context.SaveChangesAsync();

        dto.id = entity.id;
        return (true, "Inventory created", dto);
    }

    public async Task<bool> UpdateInventoryAsync(int id, Inventory dto)
    {
        var inventory = await _context.Inventories.FindAsync(id);
        if (inventory == null) return false;

        inventory.id_product = dto.id_product;
        inventory.total_amount = dto.total_amount;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteInventoryAsync(int id)
    {
        var inv = await _context.Inventories.FindAsync(id);
        if (inv == null) return false;

        _context.Inventories.Remove(inv);
        await _context.SaveChangesAsync();
        return true;
    }
}
