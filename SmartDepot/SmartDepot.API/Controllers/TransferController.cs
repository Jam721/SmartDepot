using Microsoft.AspNetCore.Mvc;
using SmartDepot.API.Dtos.Mappers;
using SmartDepot.API.Dtos.Request;
using SmartDepot.API.Dtos.Response;
using SmartDepot.Application.Interfaces.Repository;

namespace SmartDepot.API.Controllers;

[ApiController]
[Route("api/transfers")]
public class TransferController : ControllerBase
{
    private readonly ITransferRepository _repository;

    public TransferController(ITransferRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Получить список всех перемещений
    /// </summary>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Список всех зарегистрированных перемещений</returns>
    [HttpGet("get_all")]
    public async Task<IActionResult> GetTransfers(CancellationToken cancellationToken)
    {
        var transfers = await _repository.GetAllTransfersAsync(cancellationToken);

        if (transfers is null || !transfers.Any())
        {
            return NotFound(new ProblemDetails
            {
                Title = "Никто никуда не пошёл",
                Detail = "Склад стоит как вкопанный. Перемещений не найдено. Все отдыхают?",
                Status = StatusCodes.Status404NotFound
            });
        }

        var response = transfers.Select(t => t.Map(t.Id)).ToList();

        return Ok(response);
    }

    /// <summary>
    /// Создать новое перемещение
    /// </summary>
    /// <param name="request">Данные о перемещении</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Созданное перемещение</returns>
    [HttpPost("create")]
    public async Task<IActionResult> CreateTransfer([FromForm] TransferRequest request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(new ValidationProblemDetails(ModelState)
            {
                Title = "Что-то не так с формой",
                Detail = "Проверь ещё раз. Может, ты забыл указать, откуда и куда тащить?",
                Status = StatusCodes.Status400BadRequest
            });
        }

        try
        {
            var transfer = request.Map();

            var created = await _repository.CreateTransferAsync(transfer, cancellationToken);

            if (created is null)
            {
                return Problem(
                    title: "Не получилось, не фартануло",
                    detail: "Перемещение не создано. Может, коробка слишком тяжёлая?",
                    statusCode: StatusCodes.Status500InternalServerError);
            }

            var response = created.Map(created.Id);

            return Ok(response);
        }
        catch (Exception ex)
        {
            return Problem(
                title: "Ай-ай-ай",
                detail: $"Произошла непредвиденная ошибка: {ex.Message}. Мы не уверены, но возможно, это баг, а может, фича.",
                statusCode: StatusCodes.Status500InternalServerError);
        }
    }
}
