namespace Services;

using Microsoft.EntityFrameworkCore;
using ConnectionDb;
using Models;

public class OrderService
{
    private readonly AppDbContext _context;

    public OrderService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Order>> GetAllOrders()
    {
        return await _context.Orders.ToListAsync();
    }

    public async Task<Order> GetIdOrder(int id)
    {
        return await _context.Orders.FindAsync(id);
    }

    public async Task<Order> PostOrder(Order o)
    {
        _context.Orders.Add(o);
        await _context.SaveChangesAsync();
        return o;

    }

    public async Task<Order?> UpdateOrder(int id, Order o)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order == null) return null;

        order.amount = o.amount;
        order.id_inventory = o.id_inventory;

        await _context.SaveChangesAsync();
        return order;
    }
    
    public async Task<bool> DeleteOrder(int id)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order == null) return false;

        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();
        return true;

    }
}