using Microsoft.AspNetCore.Mvc;
using Swiga.Domain.Abstructions;
using Swiga.API.Contracts;
using Swiga.Domain.Models;

namespace Swiga.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InventoryController : ControllerBase
    {
        public readonly IInventoryService _inventoryService;
        public InventoryController(IInventoryService inventoryService) 
        {
            _inventoryService = inventoryService;
        }

        [HttpGet]
        public async Task<ActionResult<List<InventoryResponse>>> GetInventory()
        {
            var inventories = await _inventoryService.GetAllInventory();

            var response = inventories.Select(i => new InventoryResponse(i.Id, i.Name, i.Size, i.Gender, i.PricePerHour, i.Amount));

            return Ok(response);

        }
        [HttpPost]
        public async Task<ActionResult<Guid>> CreateInventory([FromBody] InventoryRequest request)
        {
            var (inventory, error) = InventoryModel.Create(
                Guid.NewGuid(),
                request.Name,
                request.Size,
                request.Gender,
                request.PricePerHour,
                request.Amount);

            if (!string.IsNullOrEmpty(error))
            {
                return BadRequest(error);
            }

            var inventoryId = await _inventoryService.CreateInventory(inventory);

            return Ok(inventoryId);
        }
        
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<Guid>> UpdateInventory(Guid id, [FromBody] InventoryRequest request) 
        {
            var inventoryId =  await _inventoryService.UpdateInventory(id, request.Name, request.Size, request.Gender, request.PricePerHour, request.Amount);

            return Ok(inventoryId);
        }


        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<Guid>> DeleteInventory(Guid id)
        {
            return Ok(await _inventoryService.DeleteInventory(id));
        }
    }
}
