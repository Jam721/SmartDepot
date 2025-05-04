using Microsoft.AspNetCore.Mvc;
using SmartDepot.API.Dtos.Mappers;
using SmartDepot.API.Dtos.Request;
using SmartDepot.API.Dtos.Response;
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
        
        if (warehouses is null || !warehouses.Any())
        {
            return NotFound(new ProblemDetails
            {
                Title = "Не найдено ни одного склада",
                Detail = "К сожалению, на складе ничего нет. Похоже, мы заблудились.",
                Status = StatusCodes.Status404NotFound
            });
        }

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
        {
            return NotFound(new ProblemDetails
            {
                Title = "Склад не найден",
                Detail = "Похоже, склад с таким ID не существует. Проверьте правильность данных.",
                Status = StatusCodes.Status404NotFound
            });
        }

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
        
        if (items is null || !items.Any())
        {
            return NotFound(new ProblemDetails
            {
                Title = "Предметов нет",
                Detail = "Этот склад пуст, и вещи в нем не найдены. Где же все ваши товары?",
                Status = StatusCodes.Status404NotFound
            });
        }

        var response = items.Select(i => i.Map(i.Id)).ToList();
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
        if (!ModelState.IsValid)
        {
            return ValidationProblem(new ValidationProblemDetails(ModelState)
            {
                Title = "Что-то пошло не так",
                Detail = "Пожалуйста, проверьте введённые данные. Убедитесь, что все поля заполнены правильно.",
                Status = StatusCodes.Status400BadRequest
            });
        }

        var warehouse = request.Map();
        var created = await _repository.CreateWarehouseAsync(warehouse, cancellationToken);
        
        if (created is null)
        {
            return Problem(
                title: "Не удалось создать склад",
                detail: "Не удалось создать склад. Попробуйте выбрать другое имя или ячейку.",
                statusCode: StatusCodes.Status500InternalServerError);
        }

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
    public async Task<IActionResult> UpdateWarehouse(int id, [FromForm] WarehouseRequest request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(new ValidationProblemDetails(ModelState)
            {
                Title = "Ошибка обновления",
                Detail = "Пожалуйста, проверьте введённые данные для обновления склада.",
                Status = StatusCodes.Status400BadRequest
            });
        }

        await _repository.UpdateWarehouseAsync(id, request.Map(), cancellationToken);
        return Ok("Склад успешно обновлён!");
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
        if (!ModelState.IsValid)
        {
            return ValidationProblem(new ValidationProblemDetails(ModelState)
            {
                Title = "Не удалось удалить склад",
                Detail = "Пожалуйста, проверьте правильность введённого ID и попробуйте снова.",
                Status = StatusCodes.Status400BadRequest
            });
        }

        try
        {
            await _repository.DeleteWarehouseAsync(id, cancellationToken);
            return Ok("Склад удалён! Всегда был рад помочь.");
        }
        catch (Exception e)
        {
            return BadRequest($"Произошла ошибка: {e.Message}. Склад не был удалён. Попробуйте снова.");
        }
    }
}
