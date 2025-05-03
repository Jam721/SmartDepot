using Microsoft.AspNetCore.Mvc;
using SmartDepot.API.Dtos;
using SmartDepot.API.Dtos.Mappers;
using SmartDepot.API.Dtos.Request;
using SmartDepot.Application.Interfaces.Repository;

namespace SmartDepot.API.Controllers;

[ApiController]
[Route("api/reservations")]
public class ReservationController : ControllerBase
{
    private readonly IReservationRepository _repository;

    public ReservationController(IReservationRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Получить список всех резервирований
    /// </summary>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Список всех резервирований</returns>
    [HttpGet("get_all")]
    public async Task<IActionResult> GetReservations(CancellationToken cancellationToken)
    {
        var reservations = await _repository.GetAllReservationsAsync(cancellationToken);
        
        if (reservations is null)
            return NotFound("There are no reservations");

        var response = reservations.Select(r => r.Map(r.Id)).ToList();
        
        return Ok(response);
    }

    /// <summary>
    /// Создать новое резервирование
    /// </summary>
    /// <param name="request">Данные для резервирования</param>
    /// <param name="cancellationToken">Токен отмены операции</param>
    /// <returns>Созданное резервирование</returns>
    [HttpPost("create")]
    public async Task<IActionResult> CreateReservation(
        [FromForm] ReservationRequest request,
        CancellationToken cancellationToken)
    {
        if(!ModelState.IsValid) return BadRequest(ModelState);
        
        var reservation = request.Map();
        
        var created = await _repository.CreateReservationAsync(reservation, cancellationToken);
        
        if (created is null)
            return BadRequest("Reservation could not be created");

        var response = reservation.Map(created.Id);
        
        return Ok(response);
    }
}