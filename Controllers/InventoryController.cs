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
        try{
            var createdInventory = await _InventoryService.AddInventory(inventory);
            return CreatedAtAction(nameof(CreateInventory), new { id = createdInventory.id }, createdInventory);
        }catch(Exception e){
            return StatusCode(500, new { error = e.Message });
        }
    }
};