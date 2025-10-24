namespace Services;

using ConnectionDb;
using Models;
using Microsoft.EntityFrameworkCore;
using Polly;
using Polly.CircuitBreaker;
using Microsoft.Extensions.Logging;
using Npgsql;

public class InventoryService
{
    private readonly AppDbContext _context;
    private readonly ILogger<InventoryService> _logger;

    // Circuit Breakers
    private readonly AsyncCircuitBreakerPolicy<List<Inventory>> _circuitBreakerList;
    private readonly AsyncCircuitBreakerPolicy<Inventory> _circuitBreakerSingle;

    public InventoryService(AppDbContext context, ILogger<InventoryService> logger)
    {
        _context = context;
        _logger = logger;

        // Circuit Breaker para GetAllInventories
        _circuitBreakerList = Policy<List<Inventory>>
            .Handle<DbUpdateException>()
            .Or<NpgsqlException>()  // capturar fallo de conexión
            .Or<TimeoutException>()
            .CircuitBreakerAsync(
                handledEventsAllowedBeforeBreaking: 1, // abre en el primer fallo
                durationOfBreak: TimeSpan.FromSeconds(60),
                onBreak: (ex, ts) => _logger.LogWarning($"Circuit open! DB unavailable for {ts.TotalSeconds}s."),
                onReset: () => _logger.LogWarning("Circuit closed!"),
                onHalfOpen: () => _logger.LogWarning("Circuit half-open, testing DB")
            );

        // Circuit Breaker para AddInventory
        _circuitBreakerSingle = Policy<Inventory>
            .Handle<DbUpdateException>()
            .Or<NpgsqlException>()  // capturar fallo de conexión
            .Or<TimeoutException>()
            .CircuitBreakerAsync(
                handledEventsAllowedBeforeBreaking: 1,
                durationOfBreak: TimeSpan.FromSeconds(60),
                onBreak: (ex, ts) => _logger.LogWarning($"Circuit open! DB unavailable for {ts.TotalSeconds}s."),
                onReset: () => _logger.LogWarning("Circuit closed!"),
                onHalfOpen: () => _logger.LogWarning("Circuit half-open, testing DB")
            );
    }

    public async Task<List<Inventory>> GetAllInventories()
    {
        return await _circuitBreakerList.ExecuteAsync(async () =>
        {
            return await _context.Inventories.ToListAsync();
        });
    }

    public async Task<Inventory> AddInventory(Inventory inv)
    {
        return await _circuitBreakerSingle.ExecuteAsync(async () =>
        {
            _context.Inventories.Add(inv);
            await _context.SaveChangesAsync();
            return inv;
        });
    }
}
