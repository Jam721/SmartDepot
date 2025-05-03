using Microsoft.AspNetCore.Mvc;
using SmartDepot.API.Dtos.Mappers;
using SmartDepot.API.Dtos.Request;
using SmartDepot.Application.Interfaces.Repository;
using SmartDepot.Domain.Models;

namespace SmartDepot.API.Controllers;

[ApiController]
[Route("api/items")]
public class ItemController : ControllerBase
{
    private readonly IItemRepository _repository;

    public ItemController(IItemRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Получить список всех предметов
    /// </summary>
    /// <param name="cancellationToken">Токен отмены запроса</param>
    /// <returns>Список предметов</returns>
    [HttpGet("get_all")]
    public async Task<IActionResult> GetAllItems(CancellationToken cancellationToken)
    {
        var items = await _repository.GetAllItemsAsync(cancellationToken);
        
        if (items is null)
            return NotFound("No items found");
        
        var response = items.Select(i => i.Map(i.Id)).ToList();
        
        return Ok(response);
    }

    /// <summary>
    /// Получить предмет по идентификатору
    /// </summary>
    /// <param name="id">ID предмета</param>
    /// <param name="cancellationToken">Токен отмены запроса</param>
    /// <returns>Данные предмета</returns>
    [HttpGet("get_by_id/{id}")]
    public async Task<IActionResult> GetItemById(int id, CancellationToken cancellationToken)
    {
        var item = await _repository.GetItemByIdAsync(id, cancellationToken);
        
        if (item is null)
            return NotFound("Item not found");

        var response = item.Map(item.Id);
        
        return Ok(response);
    }

    /// <summary>
    /// Получить «настроение» (состояние/тип) всех предметов
    /// </summary>
    /// <param name="cancellationToken">Токен отмены запроса</param>
    /// <returns>Список «настроений» предметов</returns>
    [HttpGet("mood")]
    public async Task<IActionResult> GetItemMood(CancellationToken cancellationToken)
    {
        var moods = await _repository.GetAllItemsMoodAsync(cancellationToken);
        
        if (moods is null)
            return NotFound("Mood not found");
        
        return Ok(moods);
    }

    /// <summary>
    /// Создать новый предмет
    /// </summary>
    /// <param name="request">Данные нового предмета</param>
    /// <param name="cancellationToken">Токен отмены запроса</param>
    /// <returns>Созданный предмет</returns>
    [HttpPost("create")]
    public async Task<IActionResult> AddItem([FromForm]ItemRequest request, CancellationToken cancellationToken)
    {
        if(!ModelState.IsValid) return BadRequest(ModelState);
        
        var item = request.Map();
        
        var created = await _repository.CreateItemAsync(item, cancellationToken);
        
        if (created is null)
            return NotFound("Item not found");

        var response = created.Map(created.Id);
        
        return Ok(response);
    }

    /// <summary>
    /// Обновить данные предмета
    /// </summary>
    /// <param name="id">ID предмета</param>
    /// <param name="request">Новые данные предмета</param>
    /// <param name="cancellationToken">Токен отмены запроса</param>
    /// <returns>Обновлённый предмет</returns>
    [HttpPut("update/{id}")]
    public async Task<IActionResult> UpdateItem(int id, [FromForm] ItemRequest request, CancellationToken cancellationToken)
    {
        if(!ModelState.IsValid) return BadRequest(ModelState);
        
        var item = request.Map();
        
        var updated = await _repository.UpdateItemAsync(id, item, cancellationToken);
        
        if (updated is null)
            return NotFound("Item not found");
        
        var response = updated.Map(updated.Id);
        
        return Ok(response);
    }

    /// <summary>
    /// Удалить предмет по ID
    /// </summary>
    /// <param name="id">ID предмета</param>
    /// <param name="cancellationToken">Токен отмены запроса</param>
    /// <returns>Результат удаления</returns>
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteItem(int id, CancellationToken cancellationToken)
    {
        if(!ModelState.IsValid) return BadRequest(ModelState);
        
        try
        {
            await _repository.DeleteItemAsync(id, cancellationToken);
            return Ok("Item deleted");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
