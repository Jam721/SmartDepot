using Microsoft.AspNetCore.Mvc;
using SmartDepot.API.Dtos.Mappers;
using SmartDepot.API.Dtos.Request;
using SmartDepot.API.Dtos.Response;
using SmartDepot.Application.Interfaces.Repository;

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

    /// <summary>Получить список всех предметов</summary>
    [HttpGet("get_all")]
    public async Task<IActionResult> GetAllItems(CancellationToken cancellationToken)
    {
        var items = await _repository.GetAllItemsAsync(cancellationToken);

        if (items is null || !items.Any())
        {
            return NotFound(new ProblemDetails
            {
                Title = "📦 Ой!",
                Detail = "Похоже, у нас закончились предметы. Хранилище опустело.",
                Status = 404
            });
        }

        var response = items.Select(i => i.Map(i.Id)).ToList();
        return Ok(response);
    }

    /// <summary>Получить предмет по идентификатору</summary>
    [HttpGet("get_by_id/{id}")]
    public async Task<IActionResult> GetItemById(int id, CancellationToken cancellationToken)
    {
        var item = await _repository.GetItemByIdAsync(id, cancellationToken);

        if (item is null)
        {
            return NotFound(new ProblemDetails
            {
                Title = "🔍 Не найдено!",
                Detail = $"Предмет с ID {id} не найден. Может, он прячется?",
                Status = 404
            });
        }

        var response = item.Map(item.Id);
        return Ok(response);
    }

    /// <summary>Получить «настроение» всех предметов</summary>
    [HttpGet("mood")]
    public async Task<IActionResult> GetItemMood(CancellationToken cancellationToken)
    {
        var moods = await _repository.GetAllItemsMoodAsync(cancellationToken);

        if (moods is null || !moods.Any())
        {
            return NotFound(new ProblemDetails
            {
                Title = "😐 Печалька",
                Detail = "Настроения предметов не найдены. Похоже, они в депрессии.",
                Status = 404
            });
        }

        return Ok(moods);
    }

    /// <summary>Создать новый предмет</summary>
    [HttpPost("create")]
    public async Task<IActionResult> AddItem([FromForm] ItemRequest request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(new ValidationProblemDetails(ModelState)
            {
                Title = "📋 Неверные данные",
                Detail = "Заполни все поля правильно, пожалуйста 🙏",
                Status = 400
            });
        }
        try
        {
            var item = request.Map();
            var created = await _repository.CreateItemAsync(item, cancellationToken);

            if (created is null)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "🚫 Не удалось создать",
                    Detail = "Не получилось добавить предмет. Он, видимо, убежал.",
                    Status = 404
                });
            }

            var response = created.Map(created.Id);
            return Ok(response);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "❌ Ошибка создания предмета",
                Detail = ex.Message,
                Status = 400
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "❗ Ошибка",
                Detail = ex.Message,
                Status = 400
            });
        }
    }

    /// <summary>Обновить данные предмета</summary>
    [HttpPut("update/{id}")]
    public async Task<IActionResult> UpdateItem(int id, [FromForm] ItemRequest request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(new ValidationProblemDetails(ModelState)
            {
                Title = "✏️ Ошибка в данных",
                Detail = "Проверь правильность заполнения полей.",
                Status = 400
            });
        }

        var item = request.Map();
        var updated = await _repository.UpdateItemAsync(id, item, cancellationToken);

        if (updated is null)
        {
            return NotFound(new ProblemDetails
            {
                Title = "🤷 Не удалось обновить",
                Detail = $"Предмет с ID {id} не найден. Обновить воздух не получится.",
                Status = 404
            });
        }

        var response = updated.Map(updated.Id);
        return Ok(response);
    }

    /// <summary>Удалить предмет по ID</summary>
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteItem(int id, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(new ValidationProblemDetails(ModelState)
            {
                Title = "🚫 Неверный запрос",
                Detail = "Невозможно удалить: данные некорректны.",
                Status = 400
            });
        }

        try
        {
            await _repository.DeleteItemAsync(id, cancellationToken);
            return Ok(new { message = $"🗑️ Успех! Предмет с ID {id} удалён." });
        }
        catch (Exception ex)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "❗ Ошибка удаления",
                Detail = ex.Message, // Текст ошибки
                Status = 400
            });
        }
    }
}
