using Infrastructure.Common.Models;
using Inventory.Product.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.Inventory;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Inventory.Product.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InventoryController(IInventoryService inventoryService) 
    : ControllerBase
{
    /// <summary>
    /// Endpoint: api/inventory/items/{itemNo}
    /// </summary>
    /// <param name="itemNo"></param>
    /// <returns></returns>
    [HttpGet("items/{itemNo}", Name = nameof(GetAllByItemNo))]
    [ProducesResponseType(typeof(IEnumerable<InventoryEntryDto>), (int) HttpStatusCode.OK)]
    public async Task<ActionResult<IEnumerable<InventoryEntryDto>>> GetAllByItemNo(
        [Required] string itemNo)
    {
        var result = await inventoryService.GetAllByItemNoAsync(itemNo);
        return Ok(result);
    }

    /// <summary>
    /// Endpoint: api/inventory/items/{itemNo}/paging
    /// </summary>
    /// <param name="itemNo"></param>
    /// <returns></returns>
    [HttpGet("items/{itemNo}/paging", Name = nameof(GetAllByItemNoPaging))]
    [ProducesResponseType(typeof(PageList<InventoryEntryDto>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<PageList<InventoryEntryDto>>> GetAllByItemNoPaging(
        [Required] string itemNo,
        [FromQuery] GetInventoryPagingQuery query)
    {
        query.SetItemNo(itemNo);
        var result = await inventoryService.GetAllByItemNoPagingAsync(query);
        return Ok(result);
    }

    /// <summary>
    /// Endpoint: api/inventory/{id}
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}", Name = nameof(GetById))]
    [ProducesResponseType(typeof(InventoryEntryDto), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<InventoryEntryDto>> GetById(
        [Required] string id)
    {
        var result = await inventoryService.GetByIdAsync(id);
        return Ok(result);
    }

    /// <summary>
    /// Endpoint: api/inventory/purchase/{itemNo}
    /// </summary>
    /// <param name="itemNo"></param>
    /// <returns></returns>
    [HttpPost("purchase/{itemNo}", Name = nameof(PurchaseOrder))]
    [ProducesResponseType(typeof(InventoryEntryDto), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<InventoryEntryDto>> PurchaseOrder(
        [Required] string itemNo,
        [FromBody] PurchaseProductDto model)
    {
        var result = await inventoryService.PurchaseItemAsync(itemNo, model);
        return Ok(result);
    }

    /// <summary>
    /// Endpoint: api/inventory/{id}
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}", Name = nameof(DeleteById))]
    [ProducesResponseType(typeof(NoContentResult), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<InventoryEntryDto>> DeleteById(
        [Required] string id)
    {
        var entity = await inventoryService.GetByIdAsync(id);
        if(entity == null)
        {
            return NotFound();
        }

        await inventoryService.DeleteAsync(id);
        return NoContent();
    }
}
