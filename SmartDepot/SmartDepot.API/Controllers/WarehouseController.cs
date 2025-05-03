using Microsoft.AspNetCore.Mvc;
using SmartDepot.API.Dtos.Mappers;
using SmartDepot.API.Dtos.Request;
using SmartDepot.Application.Interfaces.Repository;

namespace SmartDepot.API.Controllers;

[ApiController]
[Route("api/warehouses")]
public class WarehouseController : ControllerBase
{
    private readonly IWarehouseRepository _repository;

    public WarehouseController(IWarehouseRepository warehouseRepository)
    {
        _repository = warehouseRepository;
    }

    /// <summary>
    /// Получить список всех складов
    /// </summary>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Список складов</returns>
    [HttpGet("get_all")]
    public async Task<IActionResult> GetWarehouses(CancellationToken cancellationToken)
    {
        var warehouses = await _repository.GetWarehousesAsync(cancellationToken);
        
        if (warehouses is null)
            return NotFound("Warehouses not found");

        var response = warehouses.Select(w => w.Map(w.Id)).ToList();
        return Ok(response);
    }

    /// <summary>
    /// Получить склад по его ID
    /// </summary>
    /// <param name="id">Идентификатор склада</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Информация о складе</returns>
    [HttpGet("get_by_id/{id}")]
    public async Task<IActionResult> GetWarehouse(int id, CancellationToken cancellationToken)
    {
        var warehouse = await _repository.GetWarehouseByIdAsync(id, cancellationToken);
        
        if (warehouse is null)
            return NotFound("Warehouse not found");

        var response = warehouse.Map(warehouse.Id);
        return Ok(response);
    }

    /// <summary>
    /// Получить список предметов на складе по его ID
    /// </summary>
    /// <param name="id">Идентификатор склада</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Список предметов, находящихся на складе</returns>
    [HttpGet("get_items_by_id/{id}/items")]
    public async Task<IActionResult> GetItems(int id, CancellationToken cancellationToken)
    {
        var items = await _repository.GetItemsByWarehouseAsync(id, cancellationToken);
        
        if (items is null)
            return NotFound("Warehouse not found");

        var response = items.Select(i=>i.Map(i.Id)).ToList();
        return Ok(response);
    }

    /// <summary>
    /// Создание нового склада
    /// </summary>
    /// <param name="request">Данные нового склада</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Созданный склад</returns>
    [HttpPost("create")]
    public async Task<IActionResult> AddWarehouse([FromForm] WarehouseRequest request, CancellationToken cancellationToken)
    {
        if(!ModelState.IsValid) return BadRequest(ModelState);
        
        var warehouse = request.Map();
        var created = await _repository.CreateWarehouseAsync(warehouse, cancellationToken);
        
        if (created is null)
            return BadRequest("Warehouse could not be created");

        var response = warehouse.Map(created.Id);
        return Ok(response);
    }

    /// <summary>
    /// Обновление информации о складе по его ID
    /// </summary>
    /// <param name="id">Идентификатор склада</param>
    /// <param name="request">Обновлённые данные</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Результат обновления</returns>
    [HttpPut("update/{id}")]
    public async Task<IActionResult> UpdateWarehouse(
        int id,
        [FromForm] WarehouseRequest request,
        CancellationToken cancellationToken)
    {
        if(!ModelState.IsValid) return BadRequest(ModelState);
        
        var warehouse = request.Map();
        await _repository.UpdateWarehouseAsync(id, warehouse, cancellationToken);
        return Ok("Warehouse updated");
    }

    /// <summary>
    /// Удаление склада по его ID
    /// </summary>
    /// <param name="id">Идентификатор удаляемого склада</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Результат удаления</returns>
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteWarehouse(int id, CancellationToken cancellationToken)
    {
        if(!ModelState.IsValid) return BadRequest(ModelState);
        
        try
        {
            await _repository.DeleteWarehouseAsync(id, cancellationToken);
            return Ok("Warehouse deleted");
        }
        catch (Exception e)
        {
            return BadRequest($"ExMessage - {e} \n Warehouse could not be deleted");
        }
    }
}
