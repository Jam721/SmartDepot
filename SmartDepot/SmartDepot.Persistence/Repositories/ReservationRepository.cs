using Microsoft.EntityFrameworkCore;
using SmartDepot.Application.Interfaces.Repository;
using SmartDepot.Domain.Models;
using SmartDepot.Persistence.Mappers;

namespace SmartDepot.Persistence.Repositories;

public class ReservationRepository : IReservationRepository
{
    private readonly AppDbContext _context;

    public ReservationRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<Reservation>?> GetAllReservationsAsync(CancellationToken cancellationToken)
    {
        var reservations = await _context.Reservations
            .AsNoTracking()
            .OrderBy(el => el.Id)
            .Select(r=>r.Map())
            .ToListAsync(cancellationToken);
        
        return reservations;
    }

    public async Task<Reservation?> CreateReservationAsync(Reservation reservation, CancellationToken cancellationToken)
    {
        var reservationEntity = reservation.Map();
        
        await _context.Reservations.AddAsync(reservationEntity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        
        return reservationEntity.Map();
    }
}