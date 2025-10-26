using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services;
using SharedData.Models;
using System.Net.Http.Json;
using Polly;
using Polly.Extensions.Http;

namespace Controllers;

[ApiController]
[Route("api/[controller]")]
public class InventoryController : ControllerBase
{
    private readonly InventoryService _inventoryService;

    public InventoryController(InventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }

    [HttpGet]
    public async Task<IActionResult> GetInventories()
    {
        var inventories = await _inventoryService.GetAllInventoriesAsync();
        return Ok(inventories);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetInventoryById(int id)
    {
        var inventory = await _inventoryService.GetInventoryByIdAsync(id);
        if (inventory == null)
            return NotFound(new { message = "Inventory not found" });

        return Ok(inventory);
    }

    [HttpPost]
    public async Task<IActionResult> CreateInventory([FromBody] Inventory dto)
    {
        var result = await _inventoryService.AddInventoryAsync(dto);
        if (!result.success)
            return BadRequest(new { message = result.message });

        return CreatedAtAction(nameof(GetInventoryById), new { id = result.inventory!.id }, result.inventory);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateInventory(int id, [FromBody] Inventory dto)
    {
        var updated = await _inventoryService.UpdateInventoryAsync(id, dto);
        if (!updated)
            return NotFound(new { message = "Inventory not found" });

        return Ok(new { message = "Inventory updated" });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteInventory(int id)
    {
        var deleted = await _inventoryService.DeleteInventoryAsync(id);
        if (!deleted)
            return NotFound(new { message = "Inventory not found" });

        return Ok(new { message = "Inventory deleted" });
    }
}
