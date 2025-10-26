using Microsoft.EntityFrameworkCore;
using SharedData;
using SharedData.Models;

namespace Services;

public class OrderService
{
    private readonly AppDbContext _context;
    private readonly HttpClient _httpClient;

    public OrderService(AppDbContext context, HttpClient httpClient)
    {
        _context = context;
        _httpClient = httpClient;
    }

    public async Task<List<Order>> GetAllOrders()
    {
        return await _context.Orders.ToListAsync();
    }

    public async Task<Order?> GetIdOrder(int id)
    {
        return await _context.Orders.FirstOrDefaultAsync(o => o.id == id);
    }

    public async Task<Order> PostOrder(Order o)
{
    // Llamar al microservicio de inventario
    var response = await _httpClient.GetAsync($"http://localhost:5066/api/inventory/{o.id_inventory}");

    if (!response.IsSuccessStatusCode)
        throw new Exception("Inventory doesn't exist.");

    // Leer el contenido JSON y deserializarlo a Inventory
    var inv = await response.Content.ReadFromJsonAsync<Inventory>();
    if (inv == null) throw new Exception("Inventory deserialization failed.");

    if (inv.total_amount < o.amount)
        throw new Exception("Not enough inventory.");

    
    inv.total_amount -= o.amount;

    await _context.Orders.AddAsync(o);
    await _context.SaveChangesAsync();

    await _httpClient.PutAsJsonAsync($"http://localhost:5066/api/inventory/{o.id_inventory}",inv);    
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
