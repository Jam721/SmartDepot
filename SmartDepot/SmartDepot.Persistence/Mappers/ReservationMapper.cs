using SmartDepot.Domain.Models;
using SmartDepot.Persistence.Entities;

namespace SmartDepot.Persistence.Mappers;

public static class ReservationMapper
{
    public static Reservation Map(this ReservationEntity entity)
    {
        return new Reservation
        {
            Id = entity.Id,
            ItemId = entity.ItemEntityId,
            ReservedFrom = entity.ReservedFrom,
            ReservedUntil = entity.ReservedUntil
        };
    }

    public static ReservationEntity Map(this Reservation domain)
    {
        return new ReservationEntity
        {
            Id = domain.Id,
            ItemEntityId = domain.ItemId,
            ReservedFrom = domain.ReservedFrom,
            ReservedUntil = domain.ReservedUntil
        };
    }
}