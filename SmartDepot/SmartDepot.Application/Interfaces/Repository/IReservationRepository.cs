using SmartDepot.Domain.Models;

namespace SmartDepot.Application.Interfaces.Repository;

public interface IReservationRepository
{
    Task<List<Reservation>?> GetAllReservationsAsync(CancellationToken cancellationToken);
    Task<Reservation?> CreateReservationAsync(Reservation reservation, CancellationToken cancellationToken);
}