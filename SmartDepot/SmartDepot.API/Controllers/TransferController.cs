using Microsoft.AspNetCore.Mvc;
using SmartDepot.API.Dtos;
using SmartDepot.API.Dtos.Mappers;
using SmartDepot.API.Dtos.Request;
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

        if (transfers is null)
            return NotFound("No transfers found");

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
        if(!ModelState.IsValid) return BadRequest(ModelState);
        
        var transfer = request.Map();

        var created = await _repository.CreateTransferAsync(transfer, cancellationToken);

        if (created is null)
            return NotFound("Transfer was not created");

        var response = created.Map(created.Id);

        return Ok(response);
    }
}