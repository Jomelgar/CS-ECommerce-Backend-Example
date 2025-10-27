namespace Services;

using ConnectionDb;
using Models;
using Microsoft.EntityFrameworkCore;
using Polly.CircuitBreaker;
using Polly;

public class InventoryService
{
    private readonly AppDbContext _context;


    public InventoryService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Inventory>> GetAllInventories()
    {
        return await _context.Inventories.ToListAsync();
    }

    public async Task<Inventory> AddInventory(Inventory inv)
    {
        _context.Inventories.Add(inv);
        await _context.SaveChangesAsync();
        return inv;
    }

    public async Task<Inventory?> UpdateInventory(int id, Inventory inv)
    {
        var inventory = await _context.Inventories.FindAsync(id);
        if (inventory == null) return null;

        inventory.id_product = inv.id_product;
        inventory.total_amount = inv.total_amount;

        return inv;
    }

    public async Task<bool> DeleteInventory(int id)
    {
        var inventory = await _context.Inventories.FindAsync();

        if (inventory == null) return false;

        _context.Inventories.Remove(inventory);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<Inventory?> GetIdInventory(int id)
    {
        var inventory = await _context.Inventories.FindAsync(id);
        return inventory;
    }
}
