using SmartDepot.API.Dtos.Request;
using SmartDepot.API.Dtos.Response;
using SmartDepot.Domain.Models;

namespace SmartDepot.API.Dtos.Mappers;

public static class ReservationMapDto
{
    public static Reservation Map(this ReservationRequest request)
    {
        return new Reservation
        {
            ItemId = request.ItemId,
            ReservedFrom = request.ReservedFrom,
            ReservedUntil = request.ReservedUntil,
        };
    }

    public static ReservationResponse Map(this Reservation reservation, int id)
    {
        return new ReservationResponse
        {
            Id = id,
            ItemId = reservation.ItemId,
            ReservedFrom = reservation.ReservedFrom,
            ReservedUntil = reservation.ReservedUntil,
        };
    }
}