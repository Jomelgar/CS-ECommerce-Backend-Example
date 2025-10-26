namespace Controllers;

using Microsoft.AspNetCore.Mvc;
using Models;
using Services;

[ApiController]
[Route("/[controller]")]
public class InventoryController : ControllerBase
{
    private readonly InventoryService _InventoryService;

    public InventoryController(InventoryService inventoryService)
    {
        _InventoryService = inventoryService;
    }

    [HttpGet]
    public async Task<IActionResult> GetInventories()
    {
        try
        {
            var inventories = await _InventoryService.GetAllInventories();
            return Ok(inventories);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateInventory([FromBody] Inventory inventory)
    {
        try
        {
            var createdInventory = await _InventoryService.AddInventory(inventory);
            return CreatedAtAction(nameof(CreateInventory), new { id = createdInventory.id }, createdInventory);
        }
        catch (Exception e)
        {
            return StatusCode(500, new { error = e.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateInventory([FromBody] Inventory inv, [FromRoute] int id)
    {
        try
        {
            var invt = _InventoryService.UpdateInventory(id, inv);
            if (invt == null) return NotFound(new { message = "Inventory not found" });

            return Ok(inv);
        }
        catch (Exception e) { return StatusCode(500, new { error = e.Message }); }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteInventory([FromRoute] int id)
    {
        try
        {
            var isDelete = await _InventoryService.DeleteInventory(id);
            if (!isDelete) return NotFound(new { message = "Inventory not found" });
            return Ok(new { message = "Inventory deleted" });
        }
        catch (Exception e)
        {
            return StatusCode(500, new { error = e.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetInventory(int id)
    {
        try
        {
            var inv = await _InventoryService.GetIdInventory(id);
            if (inv == null) return NotFound(new { message = "Inventory not found" });
            return Ok(inv);
        }catch(Exception e){return StatusCode(500, new { error = e.Message });}
    }
};